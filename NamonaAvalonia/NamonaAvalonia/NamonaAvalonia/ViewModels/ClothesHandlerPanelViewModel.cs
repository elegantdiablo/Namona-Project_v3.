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
    public class ClothesHandlerPanelViewModel : ViewModelBase
    {
        private readonly ClientModel _model;
        private ChangeClothingDataDto _data;
        public ClothesHandlerPanelViewModel(ClientModel model, ChangeClothingDataDto data)
        {
            _model = model;
            _data = data;
            SaveChangesCommand = new RelayCommand(async () =>
            {
                await _model.EditClothes(Data);
                Saved?.Invoke(this, EventArgs.Empty);
            });
        }

        public ChangeClothingDataDto Data { get { return _data; } set { _data = value; OnPropertyChanged(nameof(Data)); } }

        public RelayCommand SaveChangesCommand { get; set; }

        public event EventHandler Saved;
    }
}
