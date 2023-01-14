using SteamAccountsManager.Models;

namespace SteamAccountsManager.ViewModels;

internal class AccountVM : BaseViewModel
{
    AccountModel accountData;

    public AccountModel AccountData
    {
        get => accountData;
        set
        {
            accountData = value;
            OnPropertyChanged(nameof(AccountData));
        }
    }


    public AccountVM(AccountModel a)
    {
        AccountData = a;
    }
}
