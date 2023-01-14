using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace SteamAccountsManager.Services;

internal static class NetworkService
{
    public static bool IsReady()
    {
        var ok = IsInternetAvailable();
        App.HAS_INTERNET = ok;
        return ok;
    }

    public static bool IsNotReady() => !IsReady();

    static bool IsInternetAvailable()
    {
        return NetworkInterface.GetIsNetworkAvailable() &&
            new Ping().Send(new IPAddress(new byte[] { 8, 8, 8, 8 }), 2000).Status == IPStatus.Success;
    }

    static bool PingHost()
    {
        var pingable = false;

        try
        {
            PingReply reply = new Ping().Send("127.0.0.1", 8080);
            pingable = reply.Status == IPStatus.Success;
        }
        catch (System.Exception e)
        {
            Debug.WriteLine(e.InnerException.Message);
        }

        return pingable;
    }
}
