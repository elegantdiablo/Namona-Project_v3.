using CommunityToolkit.Mvvm.Input;
using NamonaAvalonia.DTO;
using NamonaAvalonia.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NamonaAvalonia.ViewModels
{
    public class OrderPanelViewModel : ViewModelBase
    {
        private ClientModel _model;

        public OrderPanelViewModel(ClientModel model)
        {
            _model = model;
            OrderOC = new ObservableCollection<OrderDto>();
            EditOrderCommand = new RelayCommand<OrderDto>(p =>
            {
                CurrentPage = new OrderHandlerPanelViewModel(_model, SelectedOrder);
                var handlerVM = new OrderHandlerPanelViewModel(_model, SelectedOrder);
                handlerVM.Saved += (s, e) => { CurrentPage = this; OnPropertyChanged(nameof(CurrentPage)); };
                CurrentPage = handlerVM;
                OnPropertyChanged(nameof(CurrentPage));
            });
            DeleteOrderCommand = new RelayCommand<OrderDto>(p => { });
            CurrentPage = this;
        }

        public ObservableCollection<OrderDto> OrderOC { get; set; }
        public RelayCommand<OrderDto> EditOrderCommand { get; set; }
        public RelayCommand<OrderDto> DeleteOrderCommand { get; set; }
        public OrderDto SelectedOrder { get; set; }
        public ViewModelBase CurrentPage { get; set; }

        public async Task GetAllOrder()
        {
            if (OrderOC.Count() == 0)
            {
                List<OrderDto> orderlist = await _model.GetAllOrder();
                orderlist.ForEach(x => OrderOC.Add(x));
                OnPropertyChanged(nameof(OrderOC));
            }
        }
    }
}
