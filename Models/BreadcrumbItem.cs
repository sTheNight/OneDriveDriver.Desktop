namespace OneDriveDriver.Desktop.Models;

public record BreadcrumbItem(
    string Title,
    int SegmentCount,
    bool IsCurrent,
    bool ShowSeparator
);