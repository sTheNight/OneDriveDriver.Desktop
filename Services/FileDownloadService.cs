using OneDriveDriver.Desktop.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.Services;

public class FileDownloadService : IDisposable {
    // TODO: 将 HttpClient 改为单例注入
    private readonly HttpClient _httpClient = new();

    public async Task DownloadAsync(FileItem fileItem) {
        // TODO
    }

    public void Dispose() {
        _httpClient.Dispose();
    }
}