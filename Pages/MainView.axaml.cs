using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.ViewModels;
using System;

namespace OneDriveDriver.Desktop.Pages;

public partial class MainView : UserControl {
    public MainView() {
        InitializeComponent();
    }

    private async void OnFileItemPointerReleased(object? sender, PointerReleasedEventArgs e) {
        Console.WriteLine(1);
        if (e.InitialPressMouseButton != MouseButton.Left)
            return;

        if (sender is not Control { DataContext: FileItem fileItem })
            return;

        if (DataContext is not MainViewViewModel viewModel)
            return;

        await viewModel.OpenFileCommand.ExecuteAsync(fileItem);
    }
}