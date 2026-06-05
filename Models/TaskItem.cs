using CommunityToolkit.Mvvm.ComponentModel;

namespace OneDriveDriver.Desktop.Models;

public partial class TaskItem : ObservableObject, ITask {
    public string Title { get; set; }
}