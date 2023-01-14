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


    void AccountsSearch(object s, TextChangedEventArgs e)
    {
        var st = search.Text.Trim();

        var accs = string.IsNullOrEmpty(st)
            ? BaseViewModel.DelayedAccounts
            : BaseViewModel.DelayedAccounts.Where(a =>
            a.Id.ToString().Contains(st) || a.Login.Contains(st));

        VM.AccountsList = new ObservableCollection<AccountModel>(accs);
    }

    void AccountsSearchClear(object s, RoutedEventArgs e)
    {
        search.Text = string.Empty;
    }

    void OpenAccount(object s, RoutedEventArgs e)
    {
        var a = (s as Button).Tag;
        new AccountWindow()
        {
            DataContext = new AccountVM((AccountModel)a)
        }.Show();
    }
}
