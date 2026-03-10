using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NamonaAvalonia.Services;

namespace NamonaAvalonia.ViewModels;

public enum TableType
{
    Cart,
    Clothes,
    User
}

public enum CrudAction
{
    Add,
    Update,
    Delete
}

public partial class MainViewModel : ObservableObject
{
    private readonly IBackendService _backendService;

    public MainViewModel()
    {
        _backendService = new BackendService();
    }

    [ObservableProperty]
    private TableType selectedTable = TableType.Cart;

    [ObservableProperty]
    private CrudAction selectedAction = CrudAction.Add;

    [ObservableProperty]
    private string? id;

    [ObservableProperty]
    private string? name;

    [ObservableProperty]
    private string? type;

    [ObservableProperty]
    private string? gender;

    public bool IsDeleteSelected => SelectedAction == CrudAction.Delete;

    partial void OnSelectedActionChanged(CrudAction value)
    {
        OnPropertyChanged(nameof(IsDeleteSelected));
    }

    [RelayCommand]
    private async Task Execute()
    {
        switch (SelectedAction)
        {
            case CrudAction.Add:
                await _backendService.AddAsync(SelectedTable, new
                {
                    Id,
                    Name,
                    Type,
                    Gender
                });
                break;

            case CrudAction.Update:
                await _backendService.UpdateAsync(SelectedTable, new
                {
                    Id,
                    Name,
                    Type,
                    Gender
                });
                break;

            case CrudAction.Delete:
                await _backendService.DeleteAsync(SelectedTable, Id!);
                break;
        }
    }
}
