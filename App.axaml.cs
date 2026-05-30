using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OneDriveDriver.Desktop.Utils;
using OneDriveDriver.Desktop.ViewModels;
using OneDriveDriver.Desktop.Views;

namespace OneDriveDriver.Desktop;

public partial class App : Application {
    public override void Initialize() {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted() {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDependencyInjection();
        var serviceProvider = serviceCollection.BuildServiceProvider();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {
            desktop.MainWindow = new MainWindow {
                DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>(),
            };
        }
        base.OnFrameworkInitializationCompleted();
    }
}