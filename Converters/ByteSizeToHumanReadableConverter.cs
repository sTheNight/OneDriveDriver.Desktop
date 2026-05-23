using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace OneDriveDriver.Desktop.Converters;

public class ByteSizeToHumanReadableConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is null)
            return string.Empty;

        if (!TryConvertToDecimal(value, culture, out var bytes))
            return string.Empty;

        if (bytes < 0)
            return string.Empty;

        var units = new[] { "B", "KB", "MB", "GB", "TB" };
        var unitIndex = 0;

        while (bytes >= 1024 && unitIndex < units.Length - 1) {
            bytes /= 1024;
            unitIndex++;
        }

        return unitIndex == 0
            ? $"{bytes:0} {units[unitIndex]}"
            : $"{bytes:0.00} {units[unitIndex]}";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }

    private static bool TryConvertToDecimal(object value, CultureInfo culture, out decimal bytes) {
        switch (value) {
            case byte byteValue:
                bytes = byteValue;
                return true;
            case sbyte sbyteValue:
                bytes = sbyteValue;
                return true;
            case short shortValue:
                bytes = shortValue;
                return true;
            case ushort ushortValue:
                bytes = ushortValue;
                return true;
            case int intValue:
                bytes = intValue;
                return true;
            case uint uintValue:
                bytes = uintValue;
                return true;
            case long longValue:
                bytes = longValue;
                return true;
            case ulong ulongValue:
                bytes = ulongValue;
                return true;
            case float floatValue:
                bytes = (decimal)floatValue;
                return true;
            case double doubleValue:
                bytes = (decimal)doubleValue;
                return true;
            case decimal decimalValue:
                bytes = decimalValue;
                return true;
            case string stringValue:
                return decimal.TryParse(stringValue, NumberStyles.Number, culture, out bytes);
            default:
                bytes = 0;
                return false;
        }
    }
}
