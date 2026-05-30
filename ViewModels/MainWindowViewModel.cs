using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.Pages.ConfigView;
using OneDriveDriver.Desktop.Pages.MainView;
using OneDriveDriver.Desktop.Services;

namespace OneDriveDriver.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
    private readonly ConfigViewViewModel _configView;
    private readonly Pages.TestView.TestViewViewModel _testView;
    private readonly IUrlLauncher _urlLauncher;

    [ObservableProperty] private MainViewViewModel _mainContent;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsBottomSheetShow))]
    [NotifyPropertyChangedFor(nameof(IsOverlayShow))]
    private object? _content;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsOverlayShow))]
    private bool _isAboutDialogShow;
    public bool IsBottomSheetShow => Content != null;
    public bool IsOverlayShow => IsAboutDialogShow || IsBottomSheetShow;

    public MainWindowViewModel(
        MainViewViewModel mainView,
        ConfigViewViewModel configView,
        Pages.TestView.TestViewViewModel testView,
        IUrlLauncher urlLauncher
    ) {
        _mainContent = mainView;
        _configView = configView;
        _testView = testView;
        _urlLauncher = urlLauncher;

        WeakReferenceMessenger.Default.Register<RouteMessage>(this, OnNavigatedTo);
    }

    public void OnNavigatedTo(object recipient, RouteMessage key) {
        ViewModelBase? target = key.Key switch {
            RouteKey.ConfigView => _configView,
            RouteKey.TestView => _testView,
            _ => null
        };
        if (Content == target) return;
        Content = target;
    }

    [RelayCommand]
    public void NavigateToConfig() {
        WeakReferenceMessenger.Default.Send(new RouteMessage(RouteKey.ConfigView));
    }

    [RelayCommand]
    public void NavigateToTest() {
        WeakReferenceMessenger.Default.Send(new RouteMessage(RouteKey.TestView));
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
    public void CloseBottomSheet() {
        WeakReferenceMessenger.Default.Send(new RouteMessage(RouteKey.Close));
    }

    [RelayCommand]
    public void CloseAboutDialog() {
        IsAboutDialogShow = false;
    }
}