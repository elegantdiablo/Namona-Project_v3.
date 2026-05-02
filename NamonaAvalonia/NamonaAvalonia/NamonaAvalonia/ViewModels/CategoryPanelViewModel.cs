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
    public class CategoryPanelViewModel : ViewModelBase
    {
        private readonly ClientModel _model;
        private string _newcategory;
        private AllCategoryDto _selectedcategory;

        public CategoryPanelViewModel(ClientModel model)
        {
            _model = model;
            CategoryOC = new ObservableCollection<AllCategoryDto>();
            AddCategoryCommand = new RelayCommand(async () =>
            {
                if (!string.IsNullOrWhiteSpace(NewCategory))
                {
                    await _model.AddCategory(new AddCategoryDto
                    {
                        CategoryName = NewCategory
                    });
                    CategoryOC.Clear();
                    await GetAllCategories();
                    NewCategory = string.Empty;
                }
            });
            DeleteCategoryCommand = new RelayCommand(async () =>
            {
                if (SelectedCategory != null)
                {
                    await DeleteCategory();
                }
            });
             EditCategoryCommand = new RelayCommand(async () =>
             {
                 if (SelectedCategory != null && !string.IsNullOrWhiteSpace(NewCategory))
                 {
                     await _model.EditCategory(new EditCategoryDto
                     {
                         Id = SelectedCategory.Id,
                         CategoryName = NewCategory
                     });
                     CategoryOC.Clear();
                     await GetAllCategories();
                     NewCategory = string.Empty;
                 }
             });
        }

        public ObservableCollection<AllCategoryDto> CategoryOC { get; set; }
        public string NewCategory
        {
            get => _newcategory;
            set
            {
                if (_newcategory != value)
                {
                    _newcategory = value;
                    OnPropertyChanged(nameof(NewCategory));
                }
            }
        }

        public AllCategoryDto SelectedCategory
        {
            get => _selectedcategory;
            set
            {
                if (_selectedcategory != value)
                {
                    _selectedcategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }
        }

        public RelayCommand AddCategoryCommand { get; set; }
        public RelayCommand EditCategoryCommand { get; set; }
        public RelayCommand DeleteCategoryCommand { get; set; }
        public async Task GetAllCategories()
        {
            List<AllCategoryDto> CategoryList = await _model.GetAllCategories();
            CategoryOC = new ObservableCollection<AllCategoryDto>(CategoryList.OrderBy(x => x.CategoryName));
            OnPropertyChanged(nameof(CategoryOC));
        }

        public async Task DeleteCategory()
        {
            if (SelectedCategory != null)
            {
                await _model.DeleteCategory(SelectedCategory.Id);
                CategoryOC.Remove(SelectedCategory);
                SelectedCategory = null;
            }
        }

    }
}
