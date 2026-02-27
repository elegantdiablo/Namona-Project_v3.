using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using System.Windows.Input;
using Avalonia;

namespace NamonaAvalonia.ViewModels
{
    public class LoginViewModel : ReactiveObject
    {
        private string _password;

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = ReactiveCommand.Create(CheckLogin);
        }

        private void CheckLogin()
        {
            if (Password == "1234")
            {
                var mainWindow = (Application.Current.ApplicationLifetime
                    as IClassicDesktopStyleApplicationLifetime)?
                    .MainWindow;

                mainWindow.Content = new Views.MainView();
            }
        }
    }
}
