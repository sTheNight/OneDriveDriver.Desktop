using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using OneDriveDriver.Desktop.Models;
using System;

namespace OneDriveDriver.Desktop.Pages.MainView;

public partial class MainView : UserControl {
    private MainViewViewModel? _viewModel;
    private bool _scrollBreadcrumbsToEndPending;

    public MainView() {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e) {
        if (_viewModel is not null)
            _viewModel.BreadcrumbsChanged -= OnBreadcrumbsChanged;

        _viewModel = DataContext as MainViewViewModel;

        if (_viewModel is not null)
            _viewModel.BreadcrumbsChanged += OnBreadcrumbsChanged;
    }

    private void OnBreadcrumbsChanged(object? sender, EventArgs e) {
        _scrollBreadcrumbsToEndPending = true;
        BreadCrumbScroll.LayoutUpdated -= OnBreadCrumbScrollLayoutUpdated;
        BreadCrumbScroll.LayoutUpdated += OnBreadCrumbScrollLayoutUpdated;
        Dispatcher.UIThread.Post(ScrollBreadcrumbsToEnd, DispatcherPriority.Background);
    }

    private void OnBreadCrumbScrollLayoutUpdated(object? sender, EventArgs e) {
        ScrollBreadcrumbsToEnd();
    }

    private void ScrollBreadcrumbsToEnd() {
        if (!_scrollBreadcrumbsToEndPending)
            return;

        var scrollableWidth = Math.Max(0, BreadCrumbScroll.Extent.Width - BreadCrumbScroll.Viewport.Width);
        if (scrollableWidth == 0 && BreadCrumbScroll.Extent.Width == 0)
            return;

        BreadCrumbScroll.Offset = new Vector(scrollableWidth, BreadCrumbScroll.Offset.Y);
        BreadCrumbScroll.LayoutUpdated -= OnBreadCrumbScrollLayoutUpdated;
        _scrollBreadcrumbsToEndPending = false;
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
