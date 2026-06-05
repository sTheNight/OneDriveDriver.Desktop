using Avalonia;
using Avalonia.Controls;
using System.Windows.Input;

namespace OneDriveDriver.Desktop.Views;

public partial class TaskPanel : UserControl {
    public static readonly StyledProperty<ICommand?> CloseCommandProperty =
        AvaloniaProperty.Register<TaskPanel, ICommand?>(nameof(CloseCommand));

    public ICommand? CloseCommand {
        get => GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }

    public TaskPanel() {
        InitializeComponent();
    }
}
