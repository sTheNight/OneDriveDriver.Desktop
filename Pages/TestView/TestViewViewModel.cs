using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.Services;
using OneDriveDriver.Desktop.ViewModels;
using System.IO;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.Pages.TestView;

public partial class TestViewViewModel : ViewModelBase {
    [ObservableProperty]
    public partial bool IsPopupShow { get; set; }

    [RelayCommand]
    private static void PushToMainView() =>
        WeakReferenceMessenger.Default.Send(new RouteMessage(RouteKey.ConfigView));

    [RelayCommand]
    private async Task FileSaveTest() {
        // var bytes = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        // await _fileSaveService.SaveStreamAsFileAsync(new MemoryStream(bytes), "aaa");
    }

    [RelayCommand]
    private void TogglePopup() =>
        IsPopupShow = !IsPopupShow;
}