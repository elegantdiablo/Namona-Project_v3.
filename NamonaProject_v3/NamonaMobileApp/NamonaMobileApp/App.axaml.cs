using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using NamonaMobileApp.Model;
using NamonaMobileApp.ViewModels;
using NamonaMobileApp.Views;

namespace NamonaMobileApp
{
    public partial class App : Application
    {
        static ApiSession session;
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
             session = new ApiSession("https://localhost:????/");
            AuthModel authmodel = new AuthModel(session);
            LoginViewModel loginViewModel = new LoginViewModel(authmodel);
            MainViewModel viewModel = new MainViewModel();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
                // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
                DisableAvaloniaDataAnnotationValidation();
                var loginWindow = new LoginWindow
                {
                    DataContext = loginViewModel,
                };

                loginViewModel.SuccesLogin += async (s, e) =>
                {
                    var MainWindow = new MainWindow
                    {
                        DataContext = viewModel
                    };
                };
            }
            else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
            {
                singleViewPlatform.MainView = new MainView
                {
                    DataContext = new MainViewModel()
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
}