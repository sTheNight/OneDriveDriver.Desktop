using System;

namespace OneDriveDriver.Desktop.Models;

public record FileItem(
    string Id,
    string Name,
    ulong? Size,
    string ItemType,
    DateTimeOffset LastModified,
    uint? ChildCount,
    string? MimeType,
    string? DownloadUrl
);