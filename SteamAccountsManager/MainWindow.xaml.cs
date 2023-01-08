using SteamAccountsManager.ViewModels;
using System.Windows;

namespace SteamAccountsManager;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainVM();
    }
}
