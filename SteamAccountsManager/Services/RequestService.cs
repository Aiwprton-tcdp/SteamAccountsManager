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

        using var client = new HttpClient();
        ResponseModel<T?> response = null;

        try
        {
            response = await client.GetFromJsonAsync<ResponseModel<T?>>(url);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return default;
        }
        finally { client.Dispose(); }

        return response != null ? response.Data : default;
    }

    public static async Task<U> Post<T, U>(string url, T data)
    {
        if (NetworkService.IsNotReady()) return default;

        using var client = new HttpClient();
        HttpResponseMessage response = null;

        try
        {
            response = await client.PostAsJsonAsync(url, data);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            return default;
        }
        finally { client.Dispose(); }

        if (!response.IsSuccessStatusCode || response == null) return default;

        var result = await response.Content.ReadFromJsonAsync<ResponseModel<U>>();
        return result.Data;
    }
}
