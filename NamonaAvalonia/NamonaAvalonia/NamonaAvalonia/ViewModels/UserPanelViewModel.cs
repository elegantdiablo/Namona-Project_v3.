using CommunityToolkit.Mvvm.Input;
using NamonaAvalonia.DTO;
using NamonaAvalonia.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamonaAvalonia.ViewModels
{
    public class UserPanelViewModel : ViewModelBase
    {
        private ClientModel _model;
        private UserDto _selectedUser;

        public UserPanelViewModel(ClientModel model)
        {
            _model = model;
            UserOC = new ObservableCollection<UserDto>();
            EditUserCommand = new RelayCommand<UserDto>(p =>
            {
                CurrentPage = new UserHandlerPanelViewModel(_model, SelectedUser);
                var handlerVM = new UserHandlerPanelViewModel(_model, SelectedUser);
                handlerVM.Saved += (s, e) => { CurrentPage = this; OnPropertyChanged(nameof(CurrentPage)); };
                CurrentPage = handlerVM;
                OnPropertyChanged(nameof(CurrentPage));
            });
            DeleteUserCommand = new RelayCommand(async () =>
            {
                if (SelectedUser != null)
                {
                    await DeleteUser();
                }
            });
            CurrentPage = this;

        }

        public UserDto SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged(nameof(SelectedUser));
                }
            }
        }
        public ViewModelBase CurrentPage { get; set; }

        public ObservableCollection<UserDto> UserOC { get; set; }
        public RelayCommand<UserDto> EditUserCommand { get; set; }
        public RelayCommand DeleteUserCommand { get; set; }

        public async Task GetAllUsers()
        {
            if (UserOC == null)
            {
                UserOC = new ObservableCollection<UserDto>();
            }

            List<UserDto> userList = await _model.GetAllUsers();
            userList.ForEach(x => UserOC.Add(x));
            OnPropertyChanged(nameof(UserOC));
        }

        public async Task DeleteUser()
        {
            if (SelectedUser != null)
            {
                await _model.DeleteUser(SelectedUser.UserId);
                UserOC.Remove(SelectedUser);
                SelectedUser = null;
            }
        }
    }
}
