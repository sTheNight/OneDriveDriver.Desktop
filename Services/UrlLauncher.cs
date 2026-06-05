using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace OneDriveDriver.Desktop.Services;
public interface IUrlLauncher {
    Task<bool> LaunchAsync(string url);
}

public class UrlLauncher : IUrlLauncher {
    public async Task<bool> LaunchAsync(string url) {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
            return false;

        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return false;
        
        if (desktop.MainWindow is null)
            return false;
        
        return await desktop.MainWindow.Launcher.LaunchUriAsync(uri);
    }
}
