using NamonaAvalonia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamonaAvalonia.ViewModels
{
    public class OrderPanelViewModel : ViewModelBase
    {
        private ClientModel _model;

        public OrderPanelViewModel(ClientModel model)
        {
            _model = model;
        }
    }
}
