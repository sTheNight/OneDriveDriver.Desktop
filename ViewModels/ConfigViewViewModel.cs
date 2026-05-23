using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using OneDriveDriver.Desktop.Models;

namespace OneDriveDriver.Desktop.ViewModels;

public partial class ConfigViewViewModel : ViewModelBase {
    [RelayCommand]
    public void PushToMainView() {
        WeakReferenceMessenger.Default.Send(new RouteMessage(RouteKey.MainView));
    }
}