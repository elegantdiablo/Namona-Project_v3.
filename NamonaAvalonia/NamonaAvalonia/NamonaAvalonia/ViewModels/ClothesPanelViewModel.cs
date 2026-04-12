using NamonaAvalonia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamonaAvalonia.ViewModels
{
    public class ClothesPanelViewModel : ViewModelBase
    {
        private ClientModel _model;

        public ClothesPanelViewModel(ClientModel model)
        {
            _model = model;
        }
    }
}
