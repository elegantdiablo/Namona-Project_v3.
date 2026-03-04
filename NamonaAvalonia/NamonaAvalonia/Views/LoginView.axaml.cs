using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NamonaAvalonia.ViewModels;

namespace NamonaAvalonia;

public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
        DataContext = new LoginViewModel();
    }
}