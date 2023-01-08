using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace SteamAccountsManager.Services;

internal static partial class NetworkService
{
    [LibraryImport("wininet.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool InternetGetConnectedState(out int description, int reservedValue);


    public static bool IsReady()
    {
        return IsInternetAvailable();// && PingHost();
    }

    public static bool IsNotReady() => !IsReady();

    static bool IsInternetAvailable()
    {
        return InternetGetConnectedState(out _, 0);
    }

    static bool PingHost()
    {
        var pingable = false;
        using var pinger = new Ping();

        try
        {
            PingReply reply = pinger.Send(App.LOCALHOST);
            pingable = reply.Status == IPStatus.Success;
        }
        catch (PingException e)
        {
            Debug.WriteLine(e.InnerException.Message);
        }
        finally
        {
            pinger?.Dispose();
        }

        return pingable;
    }
}
