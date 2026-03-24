using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace NamonaAvalonia.ViewModels
{
    public partial class AdminPanelVM : ViewModelBase
    {
        public AdminPanelVM() 
        {
            CurrentPage = new DashboardViewModel();
        }

        public ViewModelBase CurrentPage { get; set; }

        public RelayCommand ToDashboardCommand => new RelayCommand(() => CurrentPage = new DashboardViewModel());
        public RelayCommand ToUserCommand => new RelayCommand(() => CurrentPage = new UserPanelViewModel());
        public RelayCommand ToOrdersCommand => new RelayCommand(() => CurrentPage = new OrderPanelViewModel());
        public RelayCommand ToClothesCommand => new RelayCommand(() => CurrentPage = new ClothesPanelViewModel());
        public RelayCommand ToCategoriesCommand => new RelayCommand(() => CurrentPage = new CategoryPanelViewModel());
        public RelayCommand ToCartCommand => new RelayCommand(() => CurrentPage = new CartPanelViewModel());




    }
}
