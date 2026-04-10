using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NamonaAvalonia.DTO;
using NamonaAvalonia.Model;

namespace NamonaAvalonia.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly ClientModel _model;

        public DashboardViewModel(ClientModel model)
        {
            _model = model;
            OrderList = new ObservableCollection<OrderDto>();
            UserList = new ObservableCollection<UserDto>();

        }

        private int _orderCount;
        private int _usercount;
        public ObservableCollection<OrderDto> OrderList { get; set; }
        public ObservableCollection<UserDto> UserList { get; set; }
        public int OrderCount { get { return _orderCount; } }
        public int UserCount { get { return _usercount; } }

        public async Task GetAllOrder()
        {
            List<OrderDto> a = await _model.GetAllOrder();
            a.OrderBy(x => x.OrderDate).Take(5).ToList().ForEach((x => OrderList.Add(x)));
            _orderCount = a.Count();
           OnPropertyChanged(nameof(OrderList));
           OnPropertyChanged(nameof(OrderCount));
        }

        public async Task GetAllUsers()
        {
            List<UserDto> b = await _model.GetAllUsers();
            _usercount = b.Count();
            OnPropertyChanged(nameof(UserCount));
            OnPropertyChanged(nameof(UserList));
        }     
    }
}
