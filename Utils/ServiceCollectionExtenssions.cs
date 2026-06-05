using Microsoft.Extensions.DependencyInjection;
using OneDriveDriver.Desktop.Pages.ConfigView;
using OneDriveDriver.Desktop.Pages.MainView;
using OneDriveDriver.Desktop.Services;
using OneDriveDriver.Desktop.Stores;
using OneDriveDriver.Desktop.ViewModels;
using TestViewViewModel = OneDriveDriver.Desktop.Pages.TestView.TestViewViewModel;

namespace OneDriveDriver.Desktop.Utils;

public static class ServiceCollectionExtensions {
    extension(ServiceCollection serviceCollection) {
        public void AddDependencyInjection() {
            // Service
            serviceCollection.AddSingleton<IUrlLauncher, UrlLauncher>();
            serviceCollection.AddSingleton<INotificationService,NotificationService>();
            serviceCollection.AddSingleton<FileDownloadService>();
            // Store
            serviceCollection.AddSingleton<FileListStore>();
            serviceCollection.AddSingleton<TaskStore>();
            // ViewModel
            serviceCollection.AddSingleton<TaskPanelViewModel>();
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddSingleton<TestViewViewModel>();
            serviceCollection.AddSingleton<MainViewViewModel>();
            serviceCollection.AddSingleton<ConfigViewViewModel>();
        }
    }
}