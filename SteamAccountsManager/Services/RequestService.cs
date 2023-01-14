using SteamAccountsManager.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SteamAccountsManager.Services;

internal static class RequestService
{
    public static async Task<T?> Get<T>(string url)
    {
        if (NetworkService.IsNotReady()) return default;
        ResponseModel<T?> response;
        try
        {
            response = await new HttpClient().GetFromJsonAsync<ResponseModel<T?>>(url);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return default;
        }

        return response.Data;
    }

    public static async Task<U> Post<T, U>(string url, T data)
    {
        if (NetworkService.IsNotReady()) return default;
        HttpResponseMessage response;
        try
        {
            response = await new HttpClient().PostAsJsonAsync(url, data);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return default;
        }

        if (response == null || !response.IsSuccessStatusCode) return default;

        var result = await response.Content.ReadFromJsonAsync<ResponseModel<U>>();
        return result.Data;
    }
}
