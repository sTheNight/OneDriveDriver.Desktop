using Avalonia;
using Avalonia.Controls;

namespace OneDriveDriver.Desktop.Controls;

public class IconButton : Button {
    public static readonly StyledProperty<string?> IconProperty =
        AvaloniaProperty.Register<IconButton, string?>(nameof(Icon));

    public string? Icon {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
}
