using Avalonia;
using Avalonia.Controls;

namespace NamonaAvalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

#if DEBUG
        this.AttachDevTools();
#endif

        Content = new LoginView();
    }
}
