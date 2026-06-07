using CommunityToolkit.Mvvm.Input;
using OneDriveDriver.Desktop.Models;
using OneDriveDriver.Desktop.Stores;
using System.Collections.ObjectModel;

namespace OneDriveDriver.Desktop.ViewModels;

public partial class TaskPanelViewModel : ViewModelBase {
    private readonly TaskStore _taskStore;

    public ObservableCollection<ITask> Tasks => _taskStore.Tasks;
    public bool IsHaveTasks => _taskStore.Tasks.Count > 0;

    public TaskPanelViewModel(TaskStore taskStore) {
        this._taskStore = taskStore;
        _taskStore.Tasks.CollectionChanged += (sender, args) => {
            OnPropertyChanged(nameof(IsHaveTasks));
        };
    }
}
