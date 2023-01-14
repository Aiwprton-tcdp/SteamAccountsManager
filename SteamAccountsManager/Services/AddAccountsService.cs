using Newtonsoft.Json;
using SteamAccountsManager.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SteamAccountsManager.Services;

internal static class AddAccountsService
{
    static string MaFilesPath { get; set; }
    static string Text { get; set; }
    static string Delimiter { get; set; } = ":";


    public static void SetDelimiter(string data)
    {
        var d = data.Trim();
        if (string.IsNullOrEmpty(d)) return;
        Delimiter = d;
    }

    public static async Task<bool> Start(string text)
    {
        Text = text;

        MFPInit();

        return await SaveData();
    }

    static void MFPInit()
    {
        var fbd = new System.Windows.Forms.FolderBrowserDialog()
        {
            Description = "Выберите каталог с maFiles",
            UseDescriptionForTitle = true
        };
        App.Current.Dispatcher.Invoke(() =>
        {
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MaFilesPath = fbd.SelectedPath;
            }
        });
    }

    static async Task<bool> SaveData()
    {
        var accounts = PrepareData();
        return accounts == default || await AccountsService.Add(accounts);
    }

    static List<AccountModel> PrepareData()
    {
        var accounts = new List<AccountModel>();

        Text.Split('\n').ToList().ForEach(line =>
        {
            var data = line.Trim().Split(Delimiter);
            if (data.Length != 2) return;

            var login = data[0].Trim();
            var password = data[1].Trim();
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                return;
            }

            accounts.Add(new AccountModel
            {
                Login = login,
                Password = password
            });
        });

        return accounts.Count == 0
            ? default
            : GetAdditionalData(accounts);
    }

    static List<AccountModel> GetAdditionalData(List<AccountModel> data)
    {
        var FullAccounts = new List<AccountModel>();
        var MaFiles = ParseMaFiles();

        data.ForEach(account =>
        {
            var mf = MaFiles.FirstOrDefault(m => Regex.IsMatch(m, account.Login.ToLower()));
            if (mf == default)
            {
                Debug.WriteLine(account.Login);
                FullAccounts.Add(account);
                return;
            }

            var json = JsonConvert.DeserializeObject<MaFileModel>(mf);
            account.SharedSecret = json.SharedSecret;
            account.SteamId = json.SteamID.ToString();
            FullAccounts.Add(account);
        });

        return FullAccounts;
    }

    static List<string> ParseMaFiles()
    {
        var MaFiles = new List<string>();
        var files = new DirectoryInfo(MaFilesPath).GetFiles("*.maFile");

        files.Where(f => f.Exists).ToList().ForEach(f =>
        {
            using var stream = new FileStream(f.FullName, FileMode.Open, FileAccess.Read);
            if (!stream.CanRead) return;

            using var reader = new StreamReader(stream);
            MaFiles.Add(reader.ReadToEnd());
        });

        return MaFiles;
    }
}
