namespace OneDriveDriver.Desktop.Models;

public class BreadcrumbItem {
    public BreadcrumbItem(string title, int segmentCount, bool isCurrent, bool showSeparator) {
        Title = title;
        SegmentCount = segmentCount;
        IsCurrent = isCurrent;
        ShowSeparator = showSeparator;
    }

    public string Title { get; }
    public int SegmentCount { get; }
    public bool IsCurrent { get; }
    public bool ShowSeparator { get; }
}