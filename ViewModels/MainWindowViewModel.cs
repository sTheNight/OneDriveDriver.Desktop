using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using OneDriveDriver.Desktop.Models;

namespace OneDriveDriver.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
    [ObservableProperty] private object? _content;

    public MainWindowViewModel() {
        WeakReferenceMessenger.Default.Register<RouteMessage>(this, OnNavigatedTo);
        OnNavigatedTo(this,new RouteMessage(RouteKey.MainView));
    }

    public void OnNavigatedTo(object recipient, RouteMessage key) {
        Content = key.Key switch {
            RouteKey.MainView => new MainViewViewModel(),
            RouteKey.ConfigView => new ConfigViewViewModel(),
            _ => null
        };
    }

    [RelayCommand]
    public void NavigateToConfig() {
        WeakReferenceMessenger.Default.Send(new RouteMessage(RouteKey.ConfigView));
    }
}