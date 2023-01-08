using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SteamAccountsManager.ViewModels;

internal class BaseViewModel : INotifyPropertyChanged
{
    public static object CurrentVM { get; set; } = new AccountsVM();


    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChangedEventHandler changed = PropertyChanged;
        if (changed == null)
        {
            return;
        }

        changed.Invoke(this, new(propertyName));
    }

    #endregion
}
