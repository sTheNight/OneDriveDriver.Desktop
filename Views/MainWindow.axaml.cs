using Avalonia.Controls;
using Avalonia.Input;
using OneDriveDriver.Desktop.ViewModels;
using System;

namespace OneDriveDriver.Desktop.Views;

public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
    }

    private void Overlay_OnPointerPressed(object? sender, PointerPressedEventArgs e) {
        if (sender is not Border { Name: "Overlay" } overlay)
            return;

        if (e.GetCurrentPoint(overlay).Properties.PointerUpdateKind is not PointerUpdateKind.LeftButtonPressed)
            return;

        if (DataContext is not MainWindowViewModel viewModel)
            return;

        viewModel.CloseAllModal();
    }
}