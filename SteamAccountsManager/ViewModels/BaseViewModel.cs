using SteamAccountsManager.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SteamAccountsManager.ViewModels;

internal class BaseViewModel : INotifyPropertyChanged
{
    public static BaseViewModel CurrentVM { get; set; } = new AccountsVM();
    public static List<AccountModel> DelayedAccounts { get; set; }


    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new(propertyName));
    }

    #endregion
}
