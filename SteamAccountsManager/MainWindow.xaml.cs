namespace SteamAccountsManager;

public partial class MainWindow : System.Windows.Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new ViewModels.MainVM();
    }
}
