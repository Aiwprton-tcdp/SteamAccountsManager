using Newtonsoft.Json;

namespace SteamAccountsManager.Models;

internal class MaFileModel
{
    class SessionModel
    {
        [JsonProperty("SteamID")]
        public long SteamID { get; set; }
    }

    [JsonProperty("shared_secret")]
    public string SharedSecret { get; set; }

    [JsonProperty("revocation_code")]
    public string RCode { get; set; }

    [JsonProperty("account_name")]
    public string AccountName { get; set; }

    public long SteamID => SessionData.SteamID;

    [JsonProperty("Session")]
    SessionModel SessionData { get; set; }
}