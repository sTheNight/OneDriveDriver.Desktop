using Avalonia.Animation.Easings;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Rendering.Composition;
using OneDriveDriver.Desktop.Theme;
using OneDriveDriver.Desktop.ViewModels;
using System;
using System.ComponentModel;
using System.Numerics;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.Views;

public partial class MainWindow : Window {
    private MainWindowViewModel? _mainWindowViewModel;
    private const uint AnimationDuration = 300;

    public MainWindow() {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e) {
        if (_mainWindowViewModel is not null)
            _mainWindowViewModel.PropertyChanged -= ViewModelOnPropertyChanged;

        _mainWindowViewModel = DataContext as MainWindowViewModel;

        if (_mainWindowViewModel is not null)
            _mainWindowViewModel.PropertyChanged += ViewModelOnPropertyChanged;
    }

    private void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
        switch (e.PropertyName) {
            case nameof(MainWindowViewModel.IsTaskPanelShow):
                _ = ShowTaskPanelAnimation();
                break;
        }
    }

    private uint _taskPanelAnimationVersion;
    private async Task ShowTaskPanelAnimation() {
        var currentAnimationVersion = _taskPanelAnimationVersion;
        if (_mainWindowViewModel is not { } viewModel)
            return;
        
        var compositionVisual = ElementComposition.GetElementVisual(TaskPanel);
        if (compositionVisual is null) return;

        var compositor = compositionVisual.Compositor;
        if (viewModel.IsTaskPanelShow) {
            TaskPanel.IsVisible = true;
            TaskPanel.IsHitTestVisible = true;
        }

        var hiddenOffset = new Vector3((float)-TaskPanel.PanelBorder.Width, 0, 0);
        var shownOffset = new Vector3(0, 0, 0);
        var easing = new CircularEaseOut();

        var slideAnimation = compositor.CreateVector3KeyFrameAnimation();
        slideAnimation.InsertKeyFrame(0f, viewModel.IsTaskPanelShow ? hiddenOffset : shownOffset, easing);
        slideAnimation.InsertKeyFrame(1f, viewModel.IsTaskPanelShow ? shownOffset : hiddenOffset, easing);
        slideAnimation.Duration = TimeSpan.FromMilliseconds(AnimationDuration);
        
        if(currentAnimationVersion != _taskPanelAnimationVersion++) return;
        compositionVisual.StartAnimation("Offset", slideAnimation);

        await Task.Delay(TimeSpan.FromMilliseconds(AnimationDuration));

        if (!viewModel.IsTaskPanelShow) {
            TaskPanel.IsVisible = false;
            TaskPanel.IsHitTestVisible = false;
        }
    }
    
    private void Overlay_OnPointerReleased(object? sender, PointerReleasedEventArgs e) {
        if (sender is not Border { Name: "Overlay" } overlay)
            return;
        
        if (e.GetCurrentPoint(overlay).Properties.PointerUpdateKind is not PointerUpdateKind.LeftButtonReleased)
            return;

        if (DataContext is not MainWindowViewModel viewModel)
            return;

        viewModel.CloseAllModal();
    }

    private void ApplyLightBlueTheme_OnClick(object? sender, RoutedEventArgs e) {
        Application.Current!.RequestedThemeVariant = AppThemeVariants.LightBlue;
    }
}
