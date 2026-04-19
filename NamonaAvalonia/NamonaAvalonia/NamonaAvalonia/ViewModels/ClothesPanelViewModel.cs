using CommunityToolkit.Mvvm.Input;
using NamonaAvalonia.DTO;
using NamonaAvalonia.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamonaAvalonia.ViewModels
{
    public class ClothesPanelViewModel : ViewModelBase
    {
        private ClientModel _model;
        private AllClothesDto _selectedclothes;

        public ClothesPanelViewModel(ClientModel model)
        {
            _model = model;
            ClothesOC = new ObservableCollection<AllClothesDto>();
            EditClothesCommand = new RelayCommand<AllClothesDto>(p =>
            {
                CurrentPage = new ClothesHandlerPanelViewModel(_model, SelectedClothes);
                var handlerVM = new ClothesHandlerPanelViewModel(_model, SelectedClothes);
                handlerVM.Saved += (s, e) => { CurrentPage = this; OnPropertyChanged(nameof(CurrentPage)); };
                CurrentPage = handlerVM;
                OnPropertyChanged(nameof(CurrentPage));
            });
        }

        public AllClothesDto SelectedClothes
        {
            get { return _selectedclothes; }
            set
            {
                _selectedclothes = value;
                OnPropertyChanged(nameof(SelectedClothes));
            }
        }

        public ObservableCollection<AllClothesDto> ClothesOC { get; set; }
        public RelayCommand<AllClothesDto> EditClothesCommand { get; set; }
        public RelayCommand DeleteClothesCommand { get; set; }
        public ViewModelBase CurrentPage { get; set; }


        public async Task GetAllClothes()
        {
            if (ClothesOC.Count == 0)
            {
                List<AllClothesDto> clothesList = await _model.GetAllClothes();
                clothesList.ForEach(x => ClothesOC.Add(x));
                OnPropertyChanged(nameof(ClothesOC));
            }
        }

        public async Task DeleteClothes()
        {
            if (SelectedClothes != null)
            {
                await _model.RemoveClothes(SelectedClothes.ClothingId);
                ClothesOC.Remove(SelectedClothes);
                OnPropertyChanged(nameof(ClothesOC));
            }

        }

    }
}
