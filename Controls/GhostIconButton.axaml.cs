using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace OneDriveDriver.Desktop.Controls;

public class GhostIconButton : Button {
    public static readonly StyledProperty<string?> IconProperty =
        AvaloniaProperty.Register<GhostIconButton, string?>(nameof(Icon));

    public static readonly StyledProperty<Color> IconColorProperty =
        AvaloniaProperty.Register<GhostIconButton, Color>(nameof(IconColor), Colors.Black);

    public string? Icon {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public Color IconColor {
        get => GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }
}
