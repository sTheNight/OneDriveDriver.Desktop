using Microsoft.Extensions.DependencyInjection;
using OneDriveDriver.Desktop.Stores;
using OneDriveDriver.Desktop.ViewModels;

namespace OneDriveDriver.Desktop.Utils;

public static class ServiceCollectionExtenssions {
    extension(ServiceCollection serviceCollection) {
        public void AddDependencyInjection() {
            serviceCollection.AddSingleton<FileListStore>();
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddTransient<MainViewViewModel>();
            serviceCollection.AddTransient<ConfigViewViewModel>();
        }
    }
}