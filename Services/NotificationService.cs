using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using OneDriveDriver.Desktop.Stores;
using OneDriveDriver.Desktop.Views;
using System;

namespace OneDriveDriver.Desktop.Services;

public interface INotificationService {
    public void ShowError(string title, string message);
    public void ShowInfo(string title, string message);
    public void ShowSuccess(string title, string message);
    public void ShowWarning(string title, string message);
}

public class NotificationService : INotificationService {
    // TODO: 将显示通知的方法收束到 Service 中而不应该由 ViewModel/Code-Behind 手动实例化 Notification 对象
    private WindowNotificationManager? _notificationManager;

    private void TryGetWindowsNotificationManager() {
        if (_notificationManager != null) return;
        if (Application.Current is null
            || Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        if (desktop.MainWindow is not MainWindow mainWindow) return;
        _notificationManager = mainWindow.NotificationManager;
    }

    public void ShowError(string title, string message) {
        TryGetWindowsNotificationManager();
        
        _notificationManager?.Show(new Notification(title, message, NotificationType.Error));
    }

    public void ShowInfo(string title, string message) {
        TryGetWindowsNotificationManager();
        
        _notificationManager?.Show(new Notification(title, message, NotificationType.Information));
    }

    public void ShowSuccess(string title, string message) {
        TryGetWindowsNotificationManager();
        
        _notificationManager?.Show(new Notification(title, message, NotificationType.Success));
    }

    public void ShowWarning(string title, string message) {
        TryGetWindowsNotificationManager();
        
        _notificationManager?.Show(new Notification(title, message, NotificationType.Warning));
    }
}