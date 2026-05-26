using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using OneDriveDriver.Desktop.Views;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.Services;

public class FileSaveService {
    public async Task SaveStreamAsFileAsync(Stream stream, string filename) {
        if (Application.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop)
            return;
        if (desktop.MainWindow is not MainWindow mainWindow) return;
        var file = await mainWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions() {
            SuggestedFileName = filename,
        });
        if (file is not null) {
            await using var fs = await file.OpenWriteAsync();
            await stream.CopyToAsync(fs);
        }
    }
}
