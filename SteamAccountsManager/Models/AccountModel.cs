using SteamAccountsManager.Controls;
using SteamAccountsManager.Services;
using System.Windows;

namespace SteamAccountsManager.Models;

internal class AccountModel
{
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
    public bool Blocked { get; set; }
    public long Balance { get; set; }
    public int Lvl { get; set; }
    public int CSGORank { get; set; }
    public string? FriendCode { get; set; }
    public bool InFarm { get; set; }


    public static Command<object> CopyData => new(s =>
    {
        App.Current.Dispatcher.Invoke(() => Clipboard.SetDataObject(s.ToString()));
    });

    public static Command<object> CopyGuardCode => new(async s =>
    {
        var code = await AccountsService.GuardCode((int)s);
        App.Current.Dispatcher.Invoke(() => Clipboard.SetDataObject(code));
    });
}
