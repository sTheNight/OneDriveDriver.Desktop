using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace OneDriveDriver.Desktop.Converters;

public class IsNullConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return IsEmpty(value);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }

    public static bool IsEmpty(object? value) {
        return value switch {
            null => true,
            string stringValue => string.IsNullOrWhiteSpace(stringValue),
            _ => false
        };
    }
}

public class IsNotNullConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        return !IsNullConverter.IsEmpty(value);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}
