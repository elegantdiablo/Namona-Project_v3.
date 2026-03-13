using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using NamonaAvalonia.Model;

namespace NamonaAvalonia.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        private readonly ClientModel _userService;

        private string _email;
        private string _password;
        private string _errorMessage;

        public event EventHandler SuccessLogin;

        public LoginViewModel(ClientModel userService)
        {
            _userService = userService;
            LoginCommand = new RelayCommand(async () => await Login());
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        private async Task Login()
        {
            try
            {
                var dto = new LoginDto
                {
                    UserName = Email,
                    Password = Password
                };

                var user = await _userService.LogIn(dto);

                if (user == null)
                {
                    ErrorMessage = "Hibás email vagy jelszó";
                    return;
                }

                ErrorMessage = "";
                // sikeres login kezelés
            }
            catch
            {
                ErrorMessage = "Hiba történt a bejelentkezés során";
            }
        }


    }
}
