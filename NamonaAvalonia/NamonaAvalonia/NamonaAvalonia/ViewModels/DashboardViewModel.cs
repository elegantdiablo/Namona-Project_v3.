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

        }

        private int _orderCount;
        public ObservableCollection<OrderDto> OrderList { get; set; }
        public int OrderCount { get { return _orderCount; } }

        public async Task GetAllOrder()
        {
            List<OrderDto> a = (await _model.GetAllOrder());
            a.OrderBy(x => x.OrderDate).Take(5).ToList().ForEach((x => OrderList.Add(x)));
            _orderCount = a.Count();
           OnPropertyChanged(nameof(OrderList));
           OnPropertyChanged(nameof(OrderCount));
        }
        
    }
}
