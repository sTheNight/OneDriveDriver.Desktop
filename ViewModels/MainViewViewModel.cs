using CommunityToolkit.Mvvm.Input;
using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.Services;
using OneDriveDriver.Desktop.Stores;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.ViewModels;

public partial class MainViewViewModel : ViewModelBase {
    private readonly FileListStore _fileListStore;
    private readonly IUrlLauncher _urlLauncher;

    public ObservableCollection<FileItem> FileList => _fileListStore.FileList;
    public ObservableCollection<BreadcrumbItem> Breadcrumbs { get; } = new();

    public bool IsLoading => _fileListStore.IsLoading;

    public string? ErrorMessage => _fileListStore.ErrorMessage;

    public MainViewViewModel(FileListStore fileListStore, IUrlLauncher urlLauncher) {
        _fileListStore = fileListStore;
        _urlLauncher = urlLauncher;
        _fileListStore.PropertyChanged += (_, e) => {
            if (e.PropertyName == nameof(FileListStore.IsLoading))
                OnPropertyChanged(nameof(IsLoading));

            if (e.PropertyName == nameof(FileListStore.ErrorMessage))
                OnPropertyChanged(nameof(ErrorMessage));
        };
        _fileListStore.Segments.CollectionChanged += (_, _) => { RebuildBreadcrumbs(); };
        RebuildBreadcrumbs();

        _ = _fileListStore.EnsureLoadedAsync();
    }

    private void RebuildBreadcrumbs() {
        Breadcrumbs.Clear();
        Breadcrumbs.Add(new BreadcrumbItem("Home", 0, _fileListStore.Segments.Count == 0, false));

        for (var i = 0; i < _fileListStore.Segments.Count; i++) {
            Breadcrumbs.Add(new BreadcrumbItem(
                _fileListStore.Segments[i],
                i + 1,
                i == _fileListStore.Segments.Count - 1,
                true));
        }
    }

    [RelayCommand]
    public async Task RefreshAsync() {
        await _fileListStore.RefreshAsync();
    }

    [RelayCommand]
    public async Task DownloadAsync(FileItem fileItem) {
        if (string.IsNullOrWhiteSpace(fileItem.DownloadUrl))
            return;

        await _urlLauncher.LaunchAsync(fileItem.DownloadUrl);
    }

    [RelayCommand]
    private async Task OpenFileAsync(FileItem fileItem) {
        if (fileItem.ItemType == "folder")
            await _fileListStore.AddSegment(fileItem.Name);
    }

    [RelayCommand]
    public async Task NavigateToBreadcrumbAsync(BreadcrumbItem breadcrumb) {
        await _fileListStore.NavigateToSegmentCount(breadcrumb.SegmentCount);
    }
}