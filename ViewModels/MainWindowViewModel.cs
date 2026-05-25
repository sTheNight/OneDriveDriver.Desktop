using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.Services;
using OneDriveDriver.Desktop.Stores;

namespace OneDriveDriver.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
    private readonly MainViewViewModel _mainView;
    private readonly ConfigViewViewModel _configView;
    private readonly IUrlLauncher _urlLauncher;

    [ObservableProperty] private object? _content;
    [ObservableProperty] private bool _isAboutDialogShow = false;

    public MainWindowViewModel(MainViewViewModel mainView, ConfigViewViewModel configView,IUrlLauncher urlLauncher) {
        _mainView = mainView;
        _configView = configView;
        _urlLauncher = urlLauncher;

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
    public async void OpenGithub() {
        await _urlLauncher.LaunchAsync("https://github.com/sTheNight/onedrive_driver_rs");
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
