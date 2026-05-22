namespace OneDriveDriver.Desktop.Models;

public class RouteMessage {
    public RouteMessage() { }

    public RouteMessage(RouteKey key) {
        Key = key;
    }
    public RouteKey Key { get; set; }
}