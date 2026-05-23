using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using OneDriveDriver.Desktop.Models;
using System;

namespace OneDriveDriver.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase {
    [ObservableProperty] private object? _content;
    private readonly IServiceProvider _serviceProvider;

    public MainWindowViewModel(IServiceProvider serviceProvider) {
        _serviceProvider = serviceProvider;
        WeakReferenceMessenger.Default.Register<RouteMessage>(this, OnNavigatedTo);
        OnNavigatedTo(this, new RouteMessage(RouteKey.MainView));
    }

    public void OnNavigatedTo(object recipient, RouteMessage key) {
        Content = key.Key switch {
            RouteKey.MainView => _serviceProvider.GetRequiredService<MainViewViewModel>(),
            RouteKey.ConfigView => _serviceProvider.GetRequiredService<ConfigViewViewModel>(),
            _ => null
        };
    }

    [RelayCommand]
    public void NavigateToConfig() {
        WeakReferenceMessenger.Default.Send(new RouteMessage(RouteKey.ConfigView));
    }
}