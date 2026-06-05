using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.Stores;
using System.Collections.ObjectModel;

namespace OneDriveDriver.Desktop.ViewModels;

public class TaskPanelViewModel : ViewModelBase {
    private readonly TaskStore taskStore;
    public ObservableCollection<ITask> Tasks => taskStore.Tasks;
    public TaskPanelViewModel(TaskStore taskStore) {
        this.taskStore = taskStore;
    }
}