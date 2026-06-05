using Avalonia.Threading;
using OneDriveDriver.Desktop.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace OneDriveDriver.Desktop.Stores;

public class TaskStore {
    public ObservableCollection<ITask> Tasks { get; } = [];

    public async Task AddTask(ITask task) {
        await Dispatcher.UIThread.InvokeAsync(() => Tasks.Add(task));
    }
}