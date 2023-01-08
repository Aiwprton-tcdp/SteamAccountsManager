using SteamAccountsManager.Controls;

namespace SteamAccountsManager.ViewModels;

internal class MainVM : BaseViewModel
{
    object сurrentPage = CurrentVM;

    public object CurrentPage
    {
        get => сurrentPage;
        set
        {
            сurrentPage = value;
            CurrentVM = value;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }

    public Command<object> ShowAccountsPageCommand => new(s => ShowAccountsPage());
    public Command<object> ShowNewAccountsPageCommand => new(s => ShowNewAccountsPage());


    void ShowAccountsPage()
    {
        if (CurrentPage.GetType() == typeof(AccountsVM)) return;
        CurrentPage = null;
        CurrentPage = new AccountsVM();
    }

    void ShowNewAccountsPage()
    {
        if (CurrentPage.GetType() == typeof(AddAccountsVM)) return;
        CurrentPage = null;
        CurrentPage = new AddAccountsVM();
    }
}
