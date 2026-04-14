using CommunityToolkit.Mvvm.Input;
using NamonaAvalonia.DTO;
using NamonaAvalonia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamonaAvalonia.ViewModels
{
    public class OrderHandlerPanelViewModel : ViewModelBase
    {
        private readonly ClientModel _model;
        private OrderDto _data;
        public OrderHandlerPanelViewModel(ClientModel model, OrderDto data)
        {
            _model = model;
            _data = data;
            SaveChangesCommand = new RelayCommand(async () =>
            {
                await _model.UpdateOrder(Data);
                Saved?.Invoke(this, EventArgs.Empty);
            });
        }

        public OrderDto Data { get { return _data; } set { _data = value; OnPropertyChanged(nameof(Data)); } }

        public RelayCommand SaveChangesCommand { get; set; }

        public event EventHandler Saved;
    }
}
