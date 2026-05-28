using Microsoft.Extensions.DependencyInjection;
using OneDriveDriver.Desktop.Services;
using OneDriveDriver.Desktop.Stores;
using OneDriveDriver.Desktop.ViewModels;

namespace OneDriveDriver.Desktop.Utils;

public static class ServiceCollectionExtenssions {
    extension(ServiceCollection serviceCollection) {
        public void AddDependencyInjection() {
            serviceCollection.AddSingleton<IUrlLauncher, UrlLauncher>();
            serviceCollection.AddSingleton<FileSaveService>();
            serviceCollection.AddSingleton<FileDownloadService>();
            serviceCollection.AddSingleton<FileListStore>();
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddSingleton<TestViewViewModel>();
            serviceCollection.AddSingleton<MainViewViewModel>();
            serviceCollection.AddSingleton<ConfigViewViewModel>();
        }
    }
}
