using System;

namespace OneDriveDriver.Desktop.Models;

public class FileItem {
    public string Id { get; set; }
    public string Name { get; set; }
    public ulong? Size { get; set; }
    public string ItemType { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public uint? ChildCount { get; set; }
    public string? MimeType { get; set; }
    public string? DownloadUrl { get; set; }
}