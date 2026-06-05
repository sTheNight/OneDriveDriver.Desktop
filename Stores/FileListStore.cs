using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;
using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.Stores;

public partial class FileListStore : BaseStore {
    private readonly HttpClient _httpClient = new() {
        BaseAddress = new Uri(Global.ENDPOINT)
    };

    private Task? _loadingTask;

    [ObservableProperty]
    public partial bool IsLoaded { get; set; }

    [ObservableProperty]
    public partial bool IsLoading { get; set; }

    [ObservableProperty]
    public partial string? ErrorMessage { get; set; }
    public ObservableCollection<FileItem> FileList { get; } = [];
    public ObservableCollection<string> Segments { get; } = [];

    public async Task EnsureLoadedAsync() {
        if (IsLoaded)
            return;

        if (_loadingTask is not null) {
            await _loadingTask;
            return;
        }

        _loadingTask = RefreshAsync();

        try {
            await _loadingTask;
        } finally {
            _loadingTask = null;
        }
    }

    public async Task RefreshAsync() {
        try {
            IsLoading = true;
            ErrorMessage = null;
            var path = BuildFullUri();
            var response = await _httpClient.GetAsync($"/api/list{path}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var fileList = JsonConvert.DeserializeObject<List<FileItem>>(content) ?? [];
            
                FileList.Clear();

                foreach (var fileItem in fileList)
                    FileList.Add(fileItem);

            IsLoaded = true;
        } catch (Exception ex) {
            ErrorMessage = ex.Message;
            FileList.Clear();
            Console.WriteLine(ex);
        } finally {
            IsLoading = false;
        }
    }

    public async Task AddSegment(string segment) {
        Segments.Add(segment);
        await RefreshAsync();
    }

    public async Task NavigateToSegmentCount(int segmentCount) {
        segmentCount = Math.Clamp(segmentCount, 0, Segments.Count);
        if (segmentCount == Segments.Count)
            return;
        
        while (Segments.Count > segmentCount)
            Segments.RemoveAt(Segments.Count - 1);

        await RefreshAsync();
    }

    private string BuildFullUri() {
        return Segments.Aggregate(string.Empty, (current, segment) => current + ("/" + segment));
    }
}
