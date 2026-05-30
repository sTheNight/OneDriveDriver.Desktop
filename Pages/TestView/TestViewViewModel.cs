using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.Services;
using OneDriveDriver.Desktop.ViewModels;
using System.IO;

namespace OneDriveDriver.Desktop.Pages.TestView;

public partial class TestViewViewModel : ViewModelBase {
    private readonly FileSaveService _fileSaveService;

    public TestViewViewModel(FileSaveService fileSaveService) {
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