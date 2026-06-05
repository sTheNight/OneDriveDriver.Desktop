using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace OneDriveDriver.Desktop.Controls;

public class Badge : TemplatedControl {
    public static readonly StyledProperty<string> BadgeTextProperty =
        AvaloniaProperty.Register<Badge, string>(nameof(BadgeText));
    public string  BadgeText {
        get => GetValue(BadgeTextProperty);
        set => SetValue(BadgeTextProperty, value);
    }
}