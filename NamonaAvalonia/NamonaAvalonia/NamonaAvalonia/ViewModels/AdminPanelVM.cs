using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using NamonaAvalonia.Model;

namespace NamonaAvalonia.ViewModels
{
    public partial class AdminPanelVM : ViewModelBase
    {
        public ClientModel _model;

        private DashboardViewModel _dashboard;

        public AdminPanelVM(ClientModel model) 
        {
            _model = model;
            _dashboard = new DashboardViewModel(_model);    
            CurrentPage = _dashboard;
            ToDashboardCommand.Execute(null);

        }

        public ViewModelBase CurrentPage { get; set; }

        public RelayCommand ToDashboardCommand => new RelayCommand(async () => { CurrentPage = _dashboard; await _dashboard.GetAllOrder() ; OnPropertyChanged(nameof(CurrentPage)); });
        public RelayCommand ToUserCommand => new RelayCommand(() => { CurrentPage = new UserPanelViewModel(); OnPropertyChanged(nameof(CurrentPage)); });
        public RelayCommand ToOrdersCommand => new RelayCommand(() => { CurrentPage = new OrderPanelViewModel(); OnPropertyChanged(nameof(CurrentPage)); });
        public RelayCommand ToClothesCommand => new RelayCommand(() => { CurrentPage = new ClothesPanelViewModel(); OnPropertyChanged(nameof(CurrentPage)); });
        public RelayCommand ToCategoriesCommand => new RelayCommand(() => { CurrentPage = new CategoryPanelViewModel(); OnPropertyChanged(nameof(CurrentPage)); });
        public RelayCommand ToCartCommand => new RelayCommand(() => { CurrentPage = new CartPanelViewModel(); OnPropertyChanged(nameof(CurrentPage)); });




    }
}
