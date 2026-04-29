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
            EditClothesCommand = new RelayCommand(() => 
            {
                if (SelectedClothes != null)
                {
                    ChangeClothingDataDto Dto = new ChangeClothingDataDto
                    {
                        ClothingId = SelectedClothes.ClothingId,
                        ClothingName = SelectedClothes.ClothingName,
                        Collection = SelectedClothes.Collection,
                        Color = SelectedClothes.Color,
                        Price = SelectedClothes.Price,
                        Size = SelectedClothes.Size,
                        Stock = SelectedClothes.Stock,
                        GenderId = SelectedClothes.GenderId,
                        CategoryId = SelectedClothes.CategoryId
                    };

                    CurrentPage = new ClothesHandlerPanelViewModel(_model, Dto);
                    var handlerVM = new ClothesHandlerPanelViewModel(_model, Dto);
                    handlerVM.Saved += (s, e) => { CurrentPage = this; OnPropertyChanged(nameof(CurrentPage)); };
                    CurrentPage = handlerVM;
                    OnPropertyChanged(nameof(CurrentPage));
                }
            });
            AddClothesCommand = new RelayCommand(async () =>
            {

                CurrentPage = new AddClothesHandlePanelViewModel(_model);

                var handlerVM = new AddClothesHandlePanelViewModel(_model);

                handlerVM.Saved += async (s, e) =>
                {
                    CurrentPage = this;
                    OnPropertyChanged(nameof(CurrentPage));
                    ClothesOC.Clear();
                    await GetAllClothes();
                };

                CurrentPage = handlerVM;
                OnPropertyChanged(nameof(CurrentPage));


            });
            DeleteClothesCommand = new RelayCommand(async () =>
            {
                if (SelectedClothes != null)
                {
                    await DeleteClothes();
                }
            });

            CurrentPage = this;
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
        public RelayCommand AddClothesCommand { get; set; }
        public RelayCommand EditClothesCommand { get; set; }
        public RelayCommand DeleteClothesCommand { get; set; }
        public ViewModelBase CurrentPage { get; set; }


        public async Task GetAllClothes()
        {
            if (ClothesOC.Count == 0)
            {
                List<AllClothesDto> clothesList = await _model.GetAllClothes();
                clothesList.ForEach(x => ClothesOC.Add(x));
                ClothesOC = new ObservableCollection<AllClothesDto>(ClothesOC.OrderBy(x => x.ClothingId));
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
