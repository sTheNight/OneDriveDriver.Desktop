using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.Services;
using System.IO;

namespace OneDriveDriver.Desktop.ViewModels;

public partial class ConfigViewViewModel : ViewModelBase {
    private readonly FileSaveService _fileSaveService;

    public ConfigViewViewModel(FileSaveService fileSaveService) {
        _fileSaveService = fileSaveService;
    }

    [RelayCommand]
    public void PushToMainView() {
        WeakReferenceMessenger.Default.Send(new RouteMessage(RouteKey.MainView));
    }

    [RelayCommand]
    public async void FileSaveTest() {
        var bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        await _fileSaveService.SaveStreamAsFileAsync(new MemoryStream(bytes),"aaa");
    }
}