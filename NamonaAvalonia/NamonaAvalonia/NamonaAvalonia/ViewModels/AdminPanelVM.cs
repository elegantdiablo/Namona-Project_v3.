using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using NamonaAvalonia.Model;

namespace NamonaAvalonia.ViewModels
{
    public class AdminPanelVM : ViewModelBase
    {
        public ClientModel _model;

        private DashboardViewModel _dashboard;
        private CategoryPanelViewModel _categorypanel;
        private OrderPanelViewModel _orderpanel;
        private UserPanelViewModel _userpanel;
        private ClothesPanelViewModel _clothespanel;

        public AdminPanelVM(ClientModel model) 
        {
            _model = model;
            _dashboard = new DashboardViewModel(_model);
            _categorypanel = new CategoryPanelViewModel(_model);
            _orderpanel = new OrderPanelViewModel(_model);
            _userpanel = new UserPanelViewModel(_model);
            _clothespanel = new ClothesPanelViewModel(_model);
            CurrentPage = _dashboard;
            ToDashboardCommand.Execute(null);

        }

        public ViewModelBase CurrentPage { get; set; }

        public RelayCommand ToDashboardCommand => new RelayCommand(async () => { CurrentPage = _dashboard; await _dashboard.GetAllOrder() ; await _dashboard.GetAllProducts();await _dashboard.GetAllUsers() ; OnPropertyChanged(nameof(CurrentPage)); });
        public RelayCommand ToUserCommand => new RelayCommand(async () => { CurrentPage = _userpanel; await _userpanel.GetAllUsers(); OnPropertyChanged(nameof(CurrentPage)); });
        public RelayCommand ToOrdersCommand => new RelayCommand(async () => { CurrentPage = _orderpanel; await _orderpanel.GetAllOrder(); OnPropertyChanged(nameof(CurrentPage)); });
        public RelayCommand ToClothesCommand => new RelayCommand(async () => { CurrentPage = _clothespanel; await _clothespanel.GetAllClothes(); OnPropertyChanged(nameof(CurrentPage)); });
        public RelayCommand ToCategoriesCommand => new RelayCommand(async () => { CurrentPage = _categorypanel; await _categorypanel.GetAllCategories(); OnPropertyChanged(nameof(CurrentPage)); });
        




    }
}
