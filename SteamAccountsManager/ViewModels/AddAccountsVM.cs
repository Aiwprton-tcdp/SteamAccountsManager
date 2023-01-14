using Microsoft.Win32;
using SteamAccountsManager.Controls;
using SteamAccountsManager.Services;
using System.IO;
using System.Threading.Tasks;

namespace SteamAccountsManager.ViewModels;

internal class AddAccountsVM : BaseViewModel
{
    string accountsText;
    bool loading;

    public string AccountsText
    {
        get => accountsText;
        set
        {
            accountsText = value;
            OnPropertyChanged(nameof(AccountsText));
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

    public Command<object> GetAcccountsCommand => new(s => GetAccounts());
    public Command<object> SaveDataCommand => new(s =>
    {
        new Task(async () => await SaveData()).Start();
    });
    public Command<object> ClearDataCommand => new(s => ClearData());


    void GetAccounts()
    {
        var openFileDialog = new OpenFileDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            var t = File.ReadAllText(openFileDialog.FileName);
            AccountsText = t.Trim();
        }
    }

    async Task SaveData()
    {
        if (string.IsNullOrEmpty(AccountsText)) return;

        Loading = true;
        
        var ok = await AddAccountsService.Start(AccountsText);
        if (!ok)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                System.Windows.Forms.MessageBox.Show("Не удалось добавить аккаунты");
            });
        }

        ClearData();
        Loading = false;
    }

    void ClearData()
    {
        AccountsText = string.Empty;
    }

}
