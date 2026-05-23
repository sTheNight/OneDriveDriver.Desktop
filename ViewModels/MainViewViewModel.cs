using CommunityToolkit.Mvvm.Input;
using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.Stores;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.ViewModels;

public partial class MainViewViewModel : ViewModelBase {
    private readonly FileListStore _fileListStore;

    public ObservableCollection<FileItem> FileList => _fileListStore.FileList;

    public bool IsLoading => _fileListStore.IsLoading;

    public string? ErrorMessage => _fileListStore.ErrorMessage;

    public MainViewViewModel(FileListStore fileListStore) {
        _fileListStore = fileListStore;
        _fileListStore.PropertyChanged += (_, e) => {
            if (e.PropertyName == nameof(FileListStore.IsLoading))
                OnPropertyChanged(nameof(IsLoading));

            if (e.PropertyName == nameof(FileListStore.ErrorMessage))
                OnPropertyChanged(nameof(ErrorMessage));
        };

        _ = _fileListStore.EnsureLoadedAsync();
    }

    [RelayCommand]
    public async Task RefreshAsync() {
        await _fileListStore.RefreshAsync();
    }

    [RelayCommand]
    public void ShowInfo(FileItem fileItem) {
        Console.WriteLine(111);
        Console.WriteLine(fileItem.Name);
    }
}
