using Microsoft.VisualBasic.Logging;
using SteamAccountsManager.Models;
using SteamAccountsManager.ViewModels;
using SteamAccountsManager.Views.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SteamAccountsManager.Views.Pages;

public partial class AccountsPage : UserControl
{
    readonly AccountsVM VM;


    public AccountsPage()
    {
        VM = BaseViewModel.CurrentVM as AccountsVM;
        InitializeComponent();
        DataContext = VM;
    }


    void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        var st = search.Text.Trim();

        if (string.IsNullOrEmpty(st))
        {
            VM.AccountsList = VM.AllAccountsList;
            return;
        }

        var accs = VM.AllAccountsList.Where(a => a.Id.ToString().Contains(st) || a.Login.Contains(st));
        VM.AccountsList = new ObservableCollection<AccountModel>(accs);
    }

    void ClearSearchButton_Click(object sender, RoutedEventArgs e)
    {
        search.Text = "";
    }

    void OpenAccount(object sender, RoutedEventArgs e)
    {
        var a = ((Button)sender).Tag;
        new AccountWindow
        {
            DataContext = (AccountModel)a
        }.Show();
    }
}
