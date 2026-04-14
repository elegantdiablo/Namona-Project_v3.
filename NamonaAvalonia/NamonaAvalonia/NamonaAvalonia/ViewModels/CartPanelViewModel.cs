using NamonaAvalonia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamonaAvalonia.ViewModels
{
    public class CartPanelViewModel : ViewModelBase
    {
        private ClientModel _model;

        public CartPanelViewModel(ClientModel model)
        {
            _model = model;
        }

        internal async Task GetAllCarts()
        {
            throw new NotImplementedException();
        }
    }
}
