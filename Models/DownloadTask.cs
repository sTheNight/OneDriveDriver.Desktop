using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;

namespace OneDriveDriver.Desktop.Models;

public partial class DownloadTask : ObservableObject, ITask {
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    public bool CanCancel => Status is DownloadTaskStatus.Pending or DownloadTaskStatus.Running;

    [ObservableProperty]
    public partial string Title { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanCancel))]
    [NotifyCanExecuteChangedFor(nameof(CancelCommand))]
    public partial DownloadTaskStatus Status { get; set; } = DownloadTaskStatus.Pending;

    public CancellationToken CancellationToken => _cancellationTokenSource.Token;
    
    [RelayCommand(CanExecute = nameof(CanCancel))]
    public void Cancel() {
        if (Status is DownloadTaskStatus.Pending or DownloadTaskStatus.Running)
            _cancellationTokenSource.Cancel();
    }
}

public enum DownloadTaskStatus {
    Pending,
    Running,
    Completed,
    Failed,
    Canceled
}
