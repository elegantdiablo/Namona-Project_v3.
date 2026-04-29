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
    public class AddClothesHandlePanelViewModel : ViewModelBase
    {
        private readonly ClientModel _model;
        private AddClothesDto _data;
        private string _clothingname;
        private string _collection;
        private string _size;
        private string _color;
        private int _price;
        private int _stock;
        private string _categoryname;
        private string _gendername;
        public string ClothingName { get { return _clothingname; } set { _clothingname = value; OnPropertyChanged(nameof(ClothingName)); } }
        public string Collection { get { return _collection; } set { _collection = value; OnPropertyChanged(nameof(Collection)); } }
        public string CategoryName { get { return _categoryname; } set { _categoryname = value; OnPropertyChanged(nameof(CategoryName)); } }
        public string Size { get { return _size; } set { _size = value; OnPropertyChanged(nameof(Size)); } }
        public string GenderName { get { return _gendername; } set { _gendername = value; OnPropertyChanged(nameof(GenderName)); } }
        public int Stock { get { return _stock; } set { _stock = value; OnPropertyChanged(nameof(Stock)); } }
        public string Color { get { return _color; } set { _color = value; OnPropertyChanged(nameof(Color)); } }
        public int Price { get { return _price; } set { _price = value; OnPropertyChanged(nameof(Price)); } }
        public AddClothesHandlePanelViewModel(ClientModel model)
        {
            _model = model;
            _data = new AddClothesDto();
            SaveChangesCommand = new RelayCommand(async () =>
            {
                _data = new AddClothesDto { ClothingName = _clothingname, Price = _price, 
                    GenderName = _gendername, CategoryName = _categoryname, Collection = _collection, 
                    Color = _color, Size = _size, Stock = _stock }; 
                await _model.AddClothes(Data);
                Saved?.Invoke(this, EventArgs.Empty);
            });
        }

        public AddClothesDto Data { get { return _data; } set { _data = value; OnPropertyChanged(nameof(Data)); } }

        public RelayCommand SaveChangesCommand { get; set; }

        public event EventHandler Saved;
    }
}
