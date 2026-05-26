using OneDriveDriver.Desktop.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.Services;

public class FileDownloadService : IDisposable {
    // TODO: 将 HttpClient 改为单例注入
    private readonly HttpClient _httpClient = new();
    private readonly FileSaveService _fileSaveService;

    public FileDownloadService(FileSaveService fileSaveService) {
        _fileSaveService = fileSaveService;
    }

    public async Task DownloadAsync(FileItem fileItem) {
        if (string.IsNullOrWhiteSpace(fileItem.DownloadUrl))
            return;

        using var response = await _httpClient.GetAsync(
            fileItem.DownloadUrl,
            HttpCompletionOption.ResponseHeadersRead
        );

        response.EnsureSuccessStatusCode();

        await using var contentStream = await response.Content.ReadAsStreamAsync();
        await _fileSaveService.SaveStreamAsFileAsync(contentStream, fileItem.Name);
    }

    public void Dispose() {
        _httpClient.Dispose();
    }
}