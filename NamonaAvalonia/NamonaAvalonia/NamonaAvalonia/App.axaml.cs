using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using NamonaAvalonia.ViewModels;
using NamonaAvalonia.Views;
using NamonaAvalonia.Model;

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
            ClientModel _model = new ClientModel("http://localhost:5222/");
            LoginViewModel viewmodel = new LoginViewModel(_model);
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
            desktop.MainWindow = new MainWindow
            {
                DataContext = viewmodel
            };
            viewmodel.SuccessLogin += (sender, args) =>
            {
                desktop.MainWindow.Content = new AdminPanel
                {
                    DataContext = new AdminPanelVM()
                };
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            ClientModel _model = new ClientModel("http://localhost:5222/");
            LoginViewModel viewmodel = new LoginViewModel(_model);
            singleViewPlatform.MainView = new MainView
            {
                DataContext = viewmodel
            };
            viewmodel.SuccessLogin += (sender, args) =>
            {
                singleViewPlatform.MainView = new AdminPanel
                {
                    DataContext = new AdminPanelVM()
                };
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}