using Avalonia;
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

    private void OnBreadcrumbPointerWheelChanged(object? sender, PointerWheelEventArgs e) {
        if (sender is not ScrollViewer scrollViewer)
            return;

        if (Math.Abs(e.Delta.X) >= Math.Abs(e.Delta.Y))
            return;

        var scrollableWidth = Math.Max(0, scrollViewer.Extent.Width - scrollViewer.Viewport.Width);
        if (scrollableWidth == 0)
            return;

        var offsetX = Math.Clamp(scrollViewer.Offset.X - e.Delta.Y * 48, 0, scrollableWidth);
        scrollViewer.Offset = new Vector(offsetX, scrollViewer.Offset.Y);
        e.Handled = true;
    }
}
