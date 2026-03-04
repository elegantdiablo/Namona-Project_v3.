using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using NamonaAvalonia.ViewModels;
using NamonaAvalonia.Views;

namespace NamonaAvalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new Window
            {
                Content = new MainView
                {
                    DataContext = new MainViewModel()
                }
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
