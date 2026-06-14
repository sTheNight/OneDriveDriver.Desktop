using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.Stores;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.Services;

public class FileDownloadService(TaskStore taskStore, INotificationService notificationService)
    : IDisposable {
    private readonly HttpClient _httpClient = new();

    public async Task DownloadAsync(FileItem fileItem) {
        var taskItem = new DownloadTask {
            Title = fileItem.Name
        };
        await taskStore.AddTask(taskItem);
        notificationService.ShowInfo("Download started", taskItem.Title);

        string? tempPath = null;

        try {
            taskItem.Status = DownloadTaskStatus.Running;

            if (string.IsNullOrWhiteSpace(fileItem.DownloadUrl))
                throw new InvalidOperationException("File does not have a download URL.");

            var downloadsPath = GetDownloadsPath();
            Directory.CreateDirectory(downloadsPath);

            var targetPath = Path.Combine(downloadsPath, GetSafeFileName(fileItem.Name));
            taskItem.DownLoadPath = targetPath;
            tempPath = $"{targetPath}.download";

            using var response = await _httpClient.GetAsync(
                fileItem.DownloadUrl,
                HttpCompletionOption.ResponseHeadersRead,
                taskItem.CancellationToken
            );
            response.EnsureSuccessStatusCode();

            await using (var contentStream = await response.Content.ReadAsStreamAsync(taskItem.CancellationToken)) {
                await using (var fileStream = File.Create(tempPath)) {
                    await contentStream.CopyToAsync(fileStream, taskItem.CancellationToken);
                }
            }

            File.Move(tempPath, targetPath, true);
            notificationService.ShowSuccess("Download completed", targetPath);
            taskItem.Status = DownloadTaskStatus.Completed;
        } catch (OperationCanceledException) {
            if (tempPath is not null && File.Exists(tempPath))
                File.Delete(tempPath);

            notificationService.ShowInfo("Download canceled", taskItem.Title);
            taskItem.Status = DownloadTaskStatus.Canceled;
        } catch (Exception e) {
            if (tempPath is not null && File.Exists(tempPath))
                File.Delete(tempPath);

            notificationService.ShowError("Download failed", e.Message);
            taskItem.Status = DownloadTaskStatus.Failed;
        }
    }
    // TODO: 这个方法不一定适用于所有系统，需要修改
    private static string GetDownloadsPath() {
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        return Path.Combine(userProfile, "Downloads");
    }

    // 替换非法字符为 _
    private static string GetSafeFileName(string fileName) =>
        Path.GetInvalidFileNameChars().Aggregate(fileName, (current, invalidChar) => current.Replace(invalidChar, '_'));


    public void Dispose() => _httpClient.Dispose();
}