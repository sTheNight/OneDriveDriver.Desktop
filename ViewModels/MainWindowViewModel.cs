using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.Pages.ConfigView;
using OneDriveDriver.Desktop.Pages.MainView;
using OneDriveDriver.Desktop.Services;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
    private readonly ConfigViewViewModel _configView;
    private readonly Pages.TestView.TestViewViewModel _testView;
    public TaskPanelViewModel TaskPanelViewModel { get; private set; }
    private readonly IUrlLauncher _urlLauncher;

    [ObservableProperty]
    public partial MainViewViewModel MainViewViewModel { get; set; }
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsBottomSheetShow))]
    [NotifyPropertyChangedFor(nameof(IsOverlayShow))]
    public partial object? Content { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsOverlayShow))]
    public partial bool IsAboutDialogShow { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsOverlayShow))]
    public partial bool IsTaskPanelShow { get; set; }

    public bool IsBottomSheetShow => Content != null;
    public bool IsOverlayShow => IsAboutDialogShow || IsBottomSheetShow || IsTaskPanelShow;

    public MainWindowViewModel(
        MainViewViewModel mainView,
        ConfigViewViewModel configView,
        Pages.TestView.TestViewViewModel testView,
        TaskPanelViewModel taskPanelViewModel,
        IUrlLauncher urlLauncher
    ) {
        MainViewViewModel = mainView;
        _configView = configView;
        _testView = testView;
        TaskPanelViewModel = taskPanelViewModel;
        _urlLauncher = urlLauncher;

        WeakReferenceMessenger.Default.Register<RouteMessage>(this, OnNavigatedTo);
    }

    private void OnNavigatedTo(object recipient, RouteMessage key) {
        ViewModelBase? target = key.Key switch {
            RouteKey.ConfigView => _configView,
            RouteKey.TestView => _testView,
            _ => null
        };
        if (Content == target) return;
        Content = target;
    }

    public void CloseAllModal() {
        if (IsAboutDialogShow) IsAboutDialogShow = false;
        if (Content != null) Content = null;
        if (IsTaskPanelShow) IsTaskPanelShow = false;
    }

    [RelayCommand]
    private void NavigateToConfig() {
        WeakReferenceMessenger.Default.Send(new RouteMessage(RouteKey.ConfigView));
    }

    [RelayCommand]
    private void NavigateToTest() {
        WeakReferenceMessenger.Default.Send(new RouteMessage(RouteKey.TestView));
    }

    [RelayCommand]
    private async Task OpenGithub() {
        await _urlLauncher.LaunchAsync("https://github.com/sTheNight/onedrive_driver_rs");
    }

    [RelayCommand]
    private void ShowAboutDialog() {
        IsAboutDialogShow = true;
    }

    [RelayCommand]
    private void CloseBottomSheet() {
        WeakReferenceMessenger.Default.Send(new RouteMessage(RouteKey.Close));
    }

    [RelayCommand]
    private void CloseAboutDialog() {
        IsAboutDialogShow = false;
    }

    [RelayCommand]
    private void ToggleTaskPanel() {
        IsTaskPanelShow = !IsTaskPanelShow;
    }

    [RelayCommand]
    private void CloseTaskPanel() {
        IsTaskPanelShow = false;
    }
}