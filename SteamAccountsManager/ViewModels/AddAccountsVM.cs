using Microsoft.Win32;
using Newtonsoft.Json;
using SteamAccountsManager.Controls;
using SteamAccountsManager.Models;
using SteamAccountsManager.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SteamAccountsManager.ViewModels;

internal class AddAccountsVM : BaseViewModel
{
    static string MaFilesPath { get; set; }
    string text;
    bool loading;

    public string Text
    {
        get => text;
        set
        {
            text = value;
            OnPropertyChanged(nameof(Text));
        }
    }
    public bool Loading
    {
        get => loading;
        set
        {
            loading = value;
            OnPropertyChanged(nameof(Loading));
        }
    }

    public Command<object> OpenFileCommand => new(s => OpenFile());
    public Command<object> SaveDataCommand => new(s => PrepareData());
    public Command<object> ClearDataCommand => new(s => ClearData());


    void OpenFile()
    {
        var openFileDialog = new OpenFileDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            Text = File.ReadAllText(openFileDialog.FileName);
        }
    }

    void PrepareData()
    {
        if (string.IsNullOrEmpty(Text)) return;

        using var fbd = new System.Windows.Forms.FolderBrowserDialog()
        {
            Description = "Выберите каталог с maFiles",
            UseDescriptionForTitle = true
        };
        if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        {
            MaFilesPath = fbd.SelectedPath;
        }

        Loading = true;
        new Task(async () => await SaveData()).Start();
    }

    void ClearData()
    {
        Text = "";
    }

    async Task SaveData()
    {
        var accounts = new List<AccountModel>();
        Text.Split('\n').ToList().ForEach(line =>
        {
            var data = line.Trim().Split(':');
            if (data.Length != 2)
            {
                return;
            }

            var login = data[0];
            var password = data[1];
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

        var fullAccounts = GetAdditionalData(accounts);
        var ok = await AccountsService.Add(fullAccounts);
        ClearData();
        Loading = false;

        if (!ok)
        {
            System.Windows.Forms.MessageBox.Show("Не удалось добавить аккаунты");
            return;
        }

        var NewAccounts = PropertiesService.Get<List<AccountModel>>("Accounts");

        if (NewAccounts == null || NewAccounts == default)
        {
            NewAccounts = fullAccounts;
        }
        else
        {
            NewAccounts.AddRange(fullAccounts);
        }

        PropertiesService.Set("Accounts", NewAccounts);
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
                return;
            }

            var json = JsonConvert.DeserializeObject<MaFileModel>(mf);
            var a = account;
            a.SharedSecret = json.SharedSecret;
            a.SteamId = json.SteamID.ToString();
            FullAccounts.Add(a);
        });

        return FullAccounts;
    }

    static List<string> ParseMaFiles()
    {
        var MaFiles = new List<string>();
        var files = new DirectoryInfo(MaFilesPath).GetFiles("*.maFile").ToList();

        files.ForEach(f =>
        {
            using var stream = new FileStream(f.FullName, FileMode.Open, FileAccess.Read);
            if (!stream.CanRead || !f.Exists)
            {
                Debug.WriteLine($"Файл {f.FullName} не найден");
                return;
            }

            using var reader = new StreamReader(stream);
            var data = reader.ReadToEnd();
            reader.Dispose();
            stream.Dispose();

            MaFiles.Add(data);
        });

        return MaFiles;
    }
}
