using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Rendering.Composition;
using OneDriveDriver.Desktop.ViewModels;
using System;
using System.ComponentModel;
using System.Numerics;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.Views;

public partial class MainWindow : Window {
    private  MainWindowViewModel _mainWindowViewModel;
    private const uint _animationDuration = 500;
    public MainWindow() {
        InitializeComponent();
        Opened += OnOpened;

    }

    private void OnOpened(object? sender, EventArgs e) {
        if (DataContext is MainWindowViewModel viewModel) {
            _mainWindowViewModel = viewModel;
            _mainWindowViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }
    }

    private void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
        Console.WriteLine(e.PropertyName);
        if (e.PropertyName == nameof(MainWindowViewModel.IsTaskPanelShow)) {
            ShowTaskPanelAnimation();
        }
    }

    private async void ShowTaskPanelAnimation() {
        var compositionVisual = ElementComposition.GetElementVisual(TaskPanel);
        if (compositionVisual is null) return;
        
        var compositor = compositionVisual.Compositor;
        if (_mainWindowViewModel.IsTaskPanelShow) {
            TaskPanel.IsVisible = true;
            TaskPanel.IsHitTestVisible = true;
        }


        var hiddenOffset = GetTaskPanelHiddnOffset();
        var shownOffset = GetTaskPanelShownOffset();
        
        var slideAnimation = compositor.CreateVector3KeyFrameAnimation();
        slideAnimation.InsertKeyFrame(0f,_mainWindowViewModel.IsTaskPanelShow ? hiddenOffset : shownOffset);
        slideAnimation.InsertKeyFrame(1f,_mainWindowViewModel.IsTaskPanelShow ? shownOffset : hiddenOffset);
        slideAnimation.Duration = TimeSpan.FromMilliseconds(_animationDuration);
        
        compositionVisual.StartAnimation("Offset", slideAnimation);

        await Task.Delay(TimeSpan.FromMilliseconds(_animationDuration));

        if (!_mainWindowViewModel.IsTaskPanelShow) {
            TaskPanel.IsVisible = false;
            TaskPanel.IsHitTestVisible = false;
        }
    }

    private Vector3 GetTaskPanelHiddnOffset() {
        return new Vector3((float)-TaskPanel.Width, 0, 0);
    }
    private Vector3 GetTaskPanelShownOffset() {
        return new Vector3(0, 0, 0);
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