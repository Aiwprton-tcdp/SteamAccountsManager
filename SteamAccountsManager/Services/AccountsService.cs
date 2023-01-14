using SteamAccountsManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamAccountsManager.Services;

internal static class AccountsService
{
    static string URL => $"{App.DOMAIN}accounts";


    public static async Task<List<AccountModel>?> Get()
    {
        return await RequestService.Get<List<AccountModel>>(URL);
    }

    public static async Task<string?> GuardCode(int id)
    {
        var url = $"{URL}/{id}/guardcode";
        return await RequestService.Get<string>(url);
    }

    public static async Task<bool> Add(List<AccountModel> data)
    {
        var count = await RequestService.Post<List<AccountModel>, int>(URL, data);
        return count > 0;
    }

    public static async Task<bool> CheckAvailability(int id)
    {
        var url = $"{URL}/{id}/check";
        return await RequestService.Get<bool>(url);
    }
}
