using Avalonia.Data.Converters;
using Avalonia.Media;
using OneDriveDriver.Desktop.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace OneDriveDriver.Desktop.Converters;

public class ItemToIconConverter : IValueConverter {
    public enum FileType {
        Archive,
        Image,
        Folder,
        Code,
        Video,
        Audio,
        Pdf,
        Document,
        Spreadsheet,
        Presentation,
        PlainText,
        Other
    }

    private const string FolderIcon = "avares://OneDriveDriver.Desktop/Assets/Icons/folder.svg";
    private const string ArchiveIcon = "avares://OneDriveDriver.Desktop/Assets/Icons/file-archive.svg";
    private const string VideoIcon = "avares://OneDriveDriver.Desktop/Assets/Icons/file-video-camera.svg";
    private const string AudioIcon = "avares://OneDriveDriver.Desktop/Assets/Icons/file-music.svg";
    private const string DocumentIcon = "avares://OneDriveDriver.Desktop/Assets/Icons/file-type.svg";
    private const string SpreadsheetIcon = "avares://OneDriveDriver.Desktop/Assets/Icons/file-spreadsheet.svg";
    private const string PresentationIcon = "avares://OneDriveDriver.Desktop/Assets/Icons/file-play.svg";
    private const string CodeIcon = "avares://OneDriveDriver.Desktop/Assets/Icons/file-braces.svg";
    private const string ImageIcon = "avares://OneDriveDriver.Desktop/Assets/Icons/file-image.svg";
    private const string PlainTextIcon = "avares://OneDriveDriver.Desktop/Assets/Icons/file-text.svg";
    private const string DefaultFileIcon = "avares://OneDriveDriver.Desktop/Assets/Icons/file.svg";

    private static readonly HashSet<string> ArchiveTypes = new(StringComparer.OrdinalIgnoreCase)
        { "zip", "7z", "rar", "xz", "gz", "tar", "tgz", "bz2", "tbz2", "zst", "br", "lz", "lzma", "cab", "iso", "dmg", "apk", "jar", "war" };

    private static readonly HashSet<string> CodeTypes = new(StringComparer.OrdinalIgnoreCase)
        { "ts", "cs", "rs", "java", "js", "kt", "py", "lua", "c", "h", "cpp", "hpp", "mjs", "mts", "tsx", "jsx", "css", "scss", "sass", "less", "html", "htm", "xhtml", "xml", "xaml", "axaml", "sql", "sh", "bash", "zsh", "ps1", "bat", "cmd", "go", "php", "rb", "swift", "dart", "fs", "fsx", "vb", "r", "scala", "clj", "ex", "exs", "erl", "hrl" };

    private static readonly HashSet<string> ImageTypes = new(StringComparer.OrdinalIgnoreCase)
        { "png", "jpg", "jpeg", "webp", "gif", "jfif", "svg", "ico", "bmp", "tif", "tiff", "avif", "heic", "heif", "raw", "psd" };

    private static readonly HashSet<string> VideoTypes = new(StringComparer.OrdinalIgnoreCase)
        { "mp4", "mkv", "mov", "avi", "wmv", "webm", "m4v", "mpg", "mpeg", "3gp", "3g2", "flv" };
    private static readonly HashSet<string> AudioTypes = new(StringComparer.OrdinalIgnoreCase)
        { "mp3", "wav", "flac", "aac", "m4a", "ogg", "oga", "opus", "wma", "mid", "midi", "aiff", "aif", "alac" };
    private static readonly HashSet<string> PdfTypes = new(StringComparer.OrdinalIgnoreCase) { "pdf" };
    private static readonly HashSet<string> DocumentTypes = new(StringComparer.OrdinalIgnoreCase)
        { "doc", "docx", "odt", "rtf", "pages","md","markdown" };
    private static readonly HashSet<string> SpreadsheetTypes = new(StringComparer.OrdinalIgnoreCase)
        { "xls", "xlsx", "xlsm", "ods", "numbers", "csv", "tsv" };
    private static readonly HashSet<string> PresentationTypes = new(StringComparer.OrdinalIgnoreCase)
        { "ppt", "pptx", "pptm", "odp", "key" };
    private static readonly HashSet<string> PlainTextTypes = new(StringComparer.OrdinalIgnoreCase)
        { "txt", "json", "yaml", "toml", "ini", "yml", "log", "conf", "config", "cfg", "env", "lock", "editorconfig" };

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is FileItem fileItem) {
            return GetFileType(fileItem) switch {
                FileType.Folder => FolderIcon,
                FileType.Archive => ArchiveIcon,
                FileType.Video => VideoIcon,
                FileType.Audio => AudioIcon,
                FileType.Pdf => DocumentIcon,
                FileType.Document => DocumentIcon,
                FileType.Spreadsheet => SpreadsheetIcon,
                FileType.Presentation => PresentationIcon,
                FileType.Code => CodeIcon,
                FileType.Image => ImageIcon,
                FileType.PlainText => PlainTextIcon,
                _ => DefaultFileIcon
            };
        }
        return null;
    }

    public static FileType GetFileType(FileItem fileItem) {
        if (string.Equals(fileItem.ItemType, "folder", StringComparison.OrdinalIgnoreCase)) {
            return FileType.Folder;
        }

        if (fileItem.MimeType?.StartsWith("image/", StringComparison.OrdinalIgnoreCase) == true) {
            return FileType.Image;
        }

        if (fileItem.MimeType?.StartsWith("video/", StringComparison.OrdinalIgnoreCase) == true) {
            return FileType.Video;
        }

        if (fileItem.MimeType?.StartsWith("audio/", StringComparison.OrdinalIgnoreCase) == true) {
            return FileType.Audio;
        }

        if (string.Equals(fileItem.MimeType, "application/pdf", StringComparison.OrdinalIgnoreCase)) {
            return FileType.Pdf;
        }

        var extension = Path.GetExtension(fileItem.Name).TrimStart('.');
        if (ArchiveTypes.Contains(extension)) {
            return FileType.Archive;
        }

        if (CodeTypes.Contains(extension)) {
            return FileType.Code;
        }

        if (ImageTypes.Contains(extension)) {
            return FileType.Image;
        }

        if (VideoTypes.Contains(extension)) {
            return FileType.Video;
        }

        if (AudioTypes.Contains(extension)) {
            return FileType.Audio;
        }

        if (PdfTypes.Contains(extension)) {
            return FileType.Pdf;
        }

        if (DocumentTypes.Contains(extension)) {
            return FileType.Document;
        }

        if (SpreadsheetTypes.Contains(extension)) {
            return FileType.Spreadsheet;
        }

        if (PresentationTypes.Contains(extension)) {
            return FileType.Presentation;
        }

        if (PlainTextTypes.Contains(extension)) {
            return FileType.PlainText;
        }

        return FileType.Other;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotSupportedException();
    }
}

public class IconColorConverter : IValueConverter {
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value is FileItem fileItem && ItemToIconConverter.GetFileType(fileItem) == ItemToIconConverter.FileType.Folder) {
            return Colors.DodgerBlue;
        }

        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotSupportedException();
    }
}
