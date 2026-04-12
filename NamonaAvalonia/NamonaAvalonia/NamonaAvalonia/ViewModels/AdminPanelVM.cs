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


        public AdminPanelVM(ClientModel model) 
        {
            _model = model;
            _dashboard = new DashboardViewModel(_model);
            _categorypanel = new CategoryPanelViewModel(_model);
            CurrentPage = _dashboard;
            ToDashboardCommand.Execute(null);

        }

        public ViewModelBase CurrentPage { get; set; }

        public RelayCommand ToDashboardCommand => new RelayCommand(async () => { CurrentPage = _dashboard; await _dashboard.GetAllOrder() ; await _dashboard.GetAllProducts();await _dashboard.GetAllUsers() ; OnPropertyChanged(nameof(CurrentPage)); });
        public RelayCommand ToUserCommand => new RelayCommand(() => { CurrentPage = new UserPanelViewModel(); OnPropertyChanged(nameof(CurrentPage)); });
        public RelayCommand ToOrdersCommand => new RelayCommand(() => { CurrentPage = new OrderPanelViewModel(); OnPropertyChanged(nameof(CurrentPage)); });
        public RelayCommand ToClothesCommand => new RelayCommand(() => { CurrentPage = new ClothesPanelViewModel(); OnPropertyChanged(nameof(CurrentPage)); });
        public RelayCommand ToCategoriesCommand => new RelayCommand(async () => { CurrentPage = _categorypanel; await _categorypanel.GetAllCategories(); OnPropertyChanged(nameof(CurrentPage)); });
        public RelayCommand ToCartCommand => new RelayCommand(() => { CurrentPage = new CartPanelViewModel(); OnPropertyChanged(nameof(CurrentPage)); });




    }
}
