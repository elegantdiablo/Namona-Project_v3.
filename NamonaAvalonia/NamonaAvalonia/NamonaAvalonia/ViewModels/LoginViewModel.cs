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
using NamonaAvalonia.DTO;

namespace NamonaAvalonia.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        private readonly ClientModel _userService;

        private string _username;
        private string _password;
        private string _errorMessage;

        public event EventHandler SuccessLogin;

        public LoginViewModel(ClientModel userService)
        {
            _userService = userService;
            LoginCommand = new RelayCommand(async () => await Login());
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value) 
                {
                    _username = value;
                    OnPropertyChanged();
                }
                
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
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                {
                    ErrorMessage = "Hibás email vagy jelszó";
                    return;
                }
                var dto = new LoginAdminDTO
                {
                    UserName = Username,
                    Password = Password
                };

                await _userService.LogIn(dto);
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
