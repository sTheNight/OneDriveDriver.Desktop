using OneDriveDriver.Desktop.Models;
using System.Collections.ObjectModel;

namespace OneDriveDriver.Desktop.Stores;

public class TaskStore {
    public ObservableCollection<ITask> Tasks { get; } = [];
}