using Avalonia.Data.Converters;
using Avalonia.Media;
using OneDriveDriver.Desktop.Models;
using System;
using System.Globalization;

namespace OneDriveDriver.Desktop.Converters;

public class ItemToIconConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is FileItem fileItem) {
            if (fileItem.ItemType == "folder") {
                return "avares://OneDriveDriver.Desktop/Assets/Icons/folder.svg";
            } else {
                return "avares://OneDriveDriver.Desktop/Assets/Icons/file.svg";
            }
        } else {
            return null;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}

public class IconColorConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is FileItem fileItem && fileItem.ItemType == "folder") {
            return Colors.DodgerBlue;
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}