using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using NamonaMobileApp.Model;

namespace NamonaMobileApp.ViewModels
{
    internal class LoginViewModel : ViewModelBase
    {
        private readonly AuthModel _loginviewModel;
        private string _email;
        private string _password;
        private string _error;
        public string Email
        {
            get { return _email; }
            set { if (value != null) _email = value; OnPropertyChanged(nameof(Email)); }
        }
        public string Password { get { return _password; } set { if (value != null) _password = value; OnPropertyChanged(nameof(Password)); } }
        public string ErrorMessage { get { return _error; } set { if (value != null) _error = value; OnPropertyChanged(nameof(ErrorMessage)); } }
        public ICommand LoginCommand { get; set; }
        public event EventHandler? SuccesLogin;
        public LoginViewModel(AuthModel loginmodel)
        {
            _loginviewModel = loginmodel;
            LoginCommand = new AsyncRelayCommand(Login);
        }

        private async Task Login()
        {
            try
            {
                await _loginviewModel.Login(Email, Password);
                if (_loginviewModel._session.Role == "Admin")
                {

                    SuccesLogin.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ErrorMessage = "Nem admin";
                }
            }
            catch (Exception ex)
            {
                _error = "No such user";
            }
        }
    }
}

