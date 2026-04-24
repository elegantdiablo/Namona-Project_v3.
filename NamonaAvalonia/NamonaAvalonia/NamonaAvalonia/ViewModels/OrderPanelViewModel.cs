using Avalonia.Controls.Platform;
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
        private OrderDto _selectedorder;

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
            DeleteOrderCommand = new RelayCommand(async () =>
            {
                if (SelectedOrder != null)
                {
                    await DeleteOrder();
                }
            });
            CompleteOrderCommand = new RelayCommand( async () => 
            {
                    await CompleteOrder();
            });
            CurrentPage = this;
        }

        public ObservableCollection<OrderDto> OrderOC { get; set; }
        public RelayCommand<OrderDto> EditOrderCommand { get; set; }
        public RelayCommand DeleteOrderCommand { get; set; }
        public RelayCommand CompleteOrderCommand { get; set; }
        public OrderDto SelectedOrder
        {
            get => _selectedorder;
            set
            {
                if (_selectedorder != value)
                {
                    _selectedorder = value;
                    OnPropertyChanged(nameof(SelectedOrder));
                }
            }
        }
        public ViewModelBase CurrentPage { get; set; }

        public async Task GetAllOrder()
        {
            if (OrderOC.Count() == 0)
            {
                List<OrderDto> orderlist = await _model.GetAllOrder();
                orderlist.ForEach(x => OrderOC.Add(x));
                OrderOC = new ObservableCollection<OrderDto>(OrderOC.OrderBy(x => x.OrderId));
                OnPropertyChanged(nameof(OrderOC));
            }
        }

        public async Task DeleteOrder() 
        { 
            if(SelectedOrder != null)
            {
                await _model.DeleteOrder(SelectedOrder.OrderId);
                OrderOC.Remove(SelectedOrder);
                SelectedOrder = null;
            }
        }

        public async Task CompleteOrder()
        {
            if(SelectedOrder != null)
            {
                await _model.CompleteOrder(SelectedOrder.OrderId);
                int UpdateId = SelectedOrder.OrderId;
                OrderOC.Remove(SelectedOrder);
                OrderDto temp = await _model.GetOrderById(UpdateId);
                OrderOC.Add(temp);
                SelectedOrder = null;
                OrderOC = new ObservableCollection<OrderDto>(OrderOC.OrderBy(x => x.OrderId));
                OrderOC.Clear();
                await GetAllOrder();
            }
        }

    }


}
