using SteamAccountsManager.Controls;
using SteamAccountsManager.Models;
using SteamAccountsManager.Services;
using SteamAccountsManager.Views.Windows;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SteamAccountsManager.ViewModels;

internal class AccountsVM : BaseViewModel
{
    ObservableCollection<AccountModel> accountsList;
    bool loading;

    public ObservableCollection<AccountModel> AccountsList
    {
        get => accountsList;
        set
        {
            accountsList = value;
            OnPropertyChanged(nameof(AccountsList));
        }
    }
    public bool Loading
    {
        get => loading;
        set
        {
            loading = value;
            OnPropertyChanged(nameof(Loading));
        }
    }

    public static Command<object> OpenAccountCommand => new(OpenAccount);


    public AccountsVM()
    {
        Loading = true;
        new Task(async () => await GetAccounts()).Start();
    }


    async Task GetAccounts()
    {
        DelayedAccounts = await AccountsService.Get()
            ?? DelayedAccounts
            ?? new();
        
        for (var i = 0; i < DelayedAccounts.Count; i++)
        {
            DelayedAccounts[i].VisualId = i+1;
        }

        AccountsList = new ObservableCollection<AccountModel>(DelayedAccounts);
        Loading = false;
    }

    static void OpenAccount(object a)
    {
        new AccountWindow()
        {
            DataContext = new AccountVM((AccountModel)a)
        }.Show();
    }
}
