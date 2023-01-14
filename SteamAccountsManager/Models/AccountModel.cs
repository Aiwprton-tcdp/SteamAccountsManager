using SteamAccountsManager.Controls;
using SteamAccountsManager.Services;
using SteamAccountsManager.ViewModels;
using System.Windows;

namespace SteamAccountsManager.Models;

internal class AccountModel : BaseViewModel
{
    bool blocked;
    int visualId;

    public int Id { get; set; }
    public int UserId { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public string? SharedSecret { get; set; }
    public string? RevocationCode { get; set; }
    public string? Email { get; set; }
    public string? SteamId { get; set; }
    public string? Nickname { get; set; }
    public bool Prime { get; set; }
    public long Balance { get; set; }
    public int Lvl { get; set; }
    public int CSGORank { get; set; }
    public string? FriendCode { get; set; }
    public bool InFarm { get; set; }
    public bool Blocked
    {
        get => blocked;
        set
        {
            blocked = value;
            OnPropertyChanged(nameof(Blocked));
        }
    }
    public int VisualId
    {
        get => visualId;
        set
        {
            visualId = value;
            OnPropertyChanged(nameof(VisualId));
        }
    }


    public static Command<object> CopyData => new(s =>
    {
        App.Current.Dispatcher.Invoke(() => Clipboard.SetDataObject(s.ToString()));
    });

    public static Command<object> CopyGuardCode => new(async id =>
    {
        var code = await AccountsService.GuardCode((int)id);
        App.Current.Dispatcher.Invoke(() => Clipboard.SetDataObject(code));
    });

    public static Command<object> CheckAvailability => new(async id =>
    {
        var code = await AccountsService.CheckAvailability((int)id);
        App.Current.Dispatcher.Invoke(() =>
        {
            System.Windows.Forms.MessageBox.Show(code ? "Валид" : "Невалид");
        });
    });
}
