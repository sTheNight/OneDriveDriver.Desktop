using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using OneDriveDriver.Desktop.Models;

namespace OneDriveDriver.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
    private readonly MainViewViewModel _mainView;
    private readonly ConfigViewViewModel _configView;

    [ObservableProperty] private object? _content;
    [ObservableProperty] private bool _isAboutDialogShow = false;

    public MainWindowViewModel(MainViewViewModel mainView, ConfigViewViewModel configView) {
        _mainView = mainView;
        _configView = configView;

        WeakReferenceMessenger.Default.Register<RouteMessage>(this, OnNavigatedTo);
        OnNavigatedTo(this, new RouteMessage(RouteKey.MainView));
    }

    public void OnNavigatedTo(object recipient, RouteMessage key) {
        Content = key.Key switch {
            RouteKey.MainView => _mainView,
            RouteKey.ConfigView => _configView,
            _ => null
        };
    }

    [RelayCommand]
    public void NavigateToConfig() {
        WeakReferenceMessenger.Default.Send(new RouteMessage(RouteKey.ConfigView));
    }

    [RelayCommand]
    public void ShowAboutDialog() {
        IsAboutDialogShow = true;
    }

    [RelayCommand]
    public void CloseAboutDialog() {
        IsAboutDialogShow = false;
    }
}
