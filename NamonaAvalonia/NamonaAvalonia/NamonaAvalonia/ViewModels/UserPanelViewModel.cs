using NamonaAvalonia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamonaAvalonia.ViewModels
{
    public class UserPanelViewModel : ViewModelBase
    {
        private ClientModel _model;

        public UserPanelViewModel(ClientModel model)
        {
            _model = model;
        }

        internal async Task GetAllUsers()
        {
            throw new NotImplementedException();
        }
    }
}
