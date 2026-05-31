using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Notifications;
using OneDriveDriver.Desktop.Stores;
using OneDriveDriver.Desktop.Views;

namespace OneDriveDriver.Desktop.Services;

public class NotificationService {
    private WindowNotificationManager? _notificationManager;

    private void TryGetWindowsNotificationManager() {
        if (Application.Current is null
            || Application.Current.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        if (desktop.MainWindow is null || desktop.MainWindow is not MainWindow mainWindow) return;
        _notificationManager = mainWindow.NotificationManager;
    }
    public void ShowNotification(Notification notification) {
        if(_notificationManager is null) TryGetWindowsNotificationManager();
        _notificationManager?.Show(notification);
    }
}