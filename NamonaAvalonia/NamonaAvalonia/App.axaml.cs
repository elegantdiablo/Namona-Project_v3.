using System.Security.Cryptography.X509Certificates;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NamonaAvalonia.Services;
using NamonaAvalonia.ViewModels;
using NamonaAvalonia.Views;

namespace NamonaAvalonia;

public partial class App : Application
{
    static ApiSession sesson;
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        sesson = new ApiSession("https://localhost:");
        AuthModel auth = new AuthModel(sesson);

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
