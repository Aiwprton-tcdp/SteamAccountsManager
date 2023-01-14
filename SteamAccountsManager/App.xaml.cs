namespace SteamAccountsManager;

public partial class App : System.Windows.Application
{
    public static string LOCALHOST => "http://127.0.0.1";
    public static string DOMAIN => $"{LOCALHOST}:3030/";
    public static bool HAS_INTERNET { get; set; }
}
