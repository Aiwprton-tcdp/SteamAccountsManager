using Newtonsoft.Json;

namespace SteamAccountsManager.Services;

internal static class PropertiesService
{
    public static void Set<T>(string name, T value)
    {
        if (!Has(name))
        {
            App.Current.Properties.Add(name, "");
        }
        App.Current.Properties[name] = JsonConvert.SerializeObject(value);
    }

    public static T Get<T>(string name)
    {
        if (!Has(name)) return default;
        var d = App.Current.Properties[name].ToString();
        return JsonConvert.DeserializeObject<T>(d);
    }

    public static bool Remove(string name)
    {
        if (!Has(name)) return false;
        App.Current.Properties.Remove(name);
        return true;
    }

    static bool Has(string name)
    {
        return !string.IsNullOrWhiteSpace(name)
            && App.Current.Properties.Contains(name);
    }
}
