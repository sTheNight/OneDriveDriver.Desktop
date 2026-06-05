using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.Stores;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.Services;

public class FileDownloadService(TaskStore taskStore) : IDisposable {
    // TODO: 将 HttpClient 改为单例注入
    private readonly HttpClient _httpClient = new();

    public async Task DownloadAsync(FileItem fileItem) {
        taskStore.AddTask(new TaskItem {
            Title = fileItem.Name,
        });
    }

    public void Dispose() {
        _httpClient.Dispose();
    }
}