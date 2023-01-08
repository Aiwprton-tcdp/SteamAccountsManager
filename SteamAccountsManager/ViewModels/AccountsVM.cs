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

    public ObservableCollection<AccountModel> AllAccountsList { get; set; }
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
        var a = await AccountsService.Get();

        AccountsList = AllAccountsList = a == null || a == default
            ? PropertiesService.Get<ObservableCollection<AccountModel>>("Accounts")
            : new ObservableCollection<AccountModel>(a);

        PropertiesService.Set("Accounts", AllAccountsList);
        Loading = false;
    }

    static void OpenAccount(object a)
    {
        new AccountWindow
        {
            DataContext = (AccountModel)a
        }.Show();
    }
}
