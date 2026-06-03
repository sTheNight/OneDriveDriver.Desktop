using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using OneDriveDriver.Desktop.Pages.ConfigView;
using OneDriveDriver.Desktop.Pages.MainView;
using OneDriveDriver.Desktop.Pages.TestView;
using OneDriveDriver.Desktop.ViewModels;

namespace OneDriveDriver.Desktop;

[RequiresUnreferencedCode(
    "Default implementation of ViewLocator involves reflection which may be trimmed away.",
    Url = "https://docs.avaloniaui.net/docs/concepts/view-locator")]
public class ViewLocator : IDataTemplate {
    public Control? Build(object? param) {
        if (param is MainViewViewModel)
            return new MainView();
        if (param is TestViewViewModel)
            return new TestView();
        if(param is ConfigViewViewModel)
            return new ConfigView();
        return new TextBlock { Text = "Not Found: " + param?.ToString() };

    }

    public bool Match(object? data) {
        return data is ViewModelBase;
    }
}