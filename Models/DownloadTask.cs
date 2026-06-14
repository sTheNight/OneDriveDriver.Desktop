using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;

namespace OneDriveDriver.Desktop.Models;

public partial class DownloadTask : ObservableObject, ITask {
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    public bool CanCancel => Status is DownloadTaskStatus.Pending or DownloadTaskStatus.Running;
    public bool CanOpenInFolder => Status is DownloadTaskStatus.Completed && DownLoadPath != null;
    public string? DownLoadPath  { get; set; }

    [ObservableProperty]
    public partial string Title { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanCancel))]
    [NotifyCanExecuteChangedFor(nameof(CancelCommand))]
    [NotifyPropertyChangedFor(nameof(CanOpenInFolder))]
    [NotifyCanExecuteChangedFor(nameof(OpenInFolderCommand))]
    public partial DownloadTaskStatus Status { get; set; } = DownloadTaskStatus.Pending;

    public CancellationToken CancellationToken => _cancellationTokenSource.Token;
    
    [RelayCommand(CanExecute = nameof(CanCancel))]
    public void Cancel() {
        if (Status is DownloadTaskStatus.Pending or DownloadTaskStatus.Running)
            _cancellationTokenSource.Cancel();
    }

    [RelayCommand(CanExecute = nameof(CanOpenInFolder))]
    public void OpenInFolder() {
        // TODO
    }
}

public enum DownloadTaskStatus {
    Pending,
    Running,
    Completed,
    Failed,
    Canceled
}
