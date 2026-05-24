using CommunityToolkit.Mvvm.Input;
using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.Services;
using OneDriveDriver.Desktop.Stores;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.ViewModels;

public partial class MainViewViewModel : ViewModelBase {
    private readonly FileListStore _fileListStore;
    private readonly IUrlLauncher _urlLauncher;

    public ObservableCollection<FileItem> FileList => _fileListStore.FileList;

    public bool IsLoading => _fileListStore.IsLoading;

    public string? ErrorMessage => _fileListStore.ErrorMessage;
    public bool CanBack => _fileListStore.CanGoBack;

    public MainViewViewModel(FileListStore fileListStore, IUrlLauncher urlLauncher) {
        _fileListStore = fileListStore;
        _urlLauncher = urlLauncher;
        _fileListStore.PropertyChanged += (_, e) => {
            if (e.PropertyName == nameof(FileListStore.IsLoading))
                OnPropertyChanged(nameof(IsLoading));

            if (e.PropertyName == nameof(FileListStore.ErrorMessage))
                OnPropertyChanged(nameof(ErrorMessage));

            if (e.PropertyName == nameof(FileListStore.CanGoBack)) {
                OnPropertyChanged(nameof(CanBack));
                BackCommand.NotifyCanExecuteChanged();
            }
        };

        _ = _fileListStore.EnsureLoadedAsync();
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

    [RelayCommand(CanExecute = nameof(CanBack))]
    public async Task BackAsync() {
        await _fileListStore.Back();
    }
}
