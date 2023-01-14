using SteamAccountsManager.Controls;
using SteamAccountsManager.Models;
using SteamAccountsManager.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SteamAccountsManager.ViewModels;

internal class MainVM : BaseViewModel
{
    BaseViewModel сurrentPage = CurrentVM;
    bool isChecking;
    static CancellationTokenSource StopChecking { get; set; }
    static CancellationToken StopCheckingToken { get; set; }

    public BaseViewModel CurrentPage
    {
        get => сurrentPage;
        set
        {
            сurrentPage = value;
            CurrentVM = value;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }
    public bool IsChecking
    {
        get => isChecking;
        set
        {
            isChecking = value;
            OnPropertyChanged(nameof(IsChecking));
        }
    }

    public Command<object> ShowAccountsPageCommand => new(s => ShowAccountsPage());
    public Command<object> ShowNewAccountsPageCommand => new(s => ShowNewAccountsPage());
    public Command<object> CheckAccountsCommand => new(s => GoToCheckAccounts());


    void ShowAccountsPage()
    {
        if (CurrentPage.GetType() == typeof(AccountsVM)) return;
        CurrentPage = null;
        CurrentPage = new AccountsVM();
    }

    void ShowNewAccountsPage()
    {
        if (CurrentPage.GetType() == typeof(AddAccountsVM)) return;
        CurrentPage = null;
        CurrentPage = new AddAccountsVM();
    }

    void GoToCheckAccounts()
    {
        if (DelayedAccounts.Count == 0)
        {
            System.Windows.Forms.MessageBox.Show("Нет аккаунтов для проверки");
            return;
        }
        if (NetworkService.IsNotReady())
        {
            System.Windows.Forms.MessageBox.Show("Нет доступа к интернету");
            return;
        }
        
        IsChecking = !IsChecking;
        if (!IsChecking)
        {
            StopChecking.Cancel();
            return;
        }

        StopChecking = new();
        StopCheckingToken = StopChecking.Token;
        new Task(async () =>
        {
            await CheckAccounts(DelayedAccounts).ConfigureAwait(false);
            IsChecking = !IsChecking;
        }, StopCheckingToken, TaskCreationOptions.LongRunning).Start();
    }

    static async Task CheckAccounts(List<AccountModel> accounts, int count = 0)
    {
        if (accounts == null) return;
        var problematic = new List<AccountModel>();

        foreach (var da in accounts)
        {
            if (StopCheckingToken.IsCancellationRequested) return;
            if (!App.HAS_INTERNET)
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    System.Windows.Forms.MessageBox.Show("Нет доступа к интернету");
                });
                return;
            }

            var g = await AccountsService.CheckAvailability(da.Id);
            if (da.Blocked = !g)
            {
                Debug.WriteLine(da.Id + ": " + da.Login);
                problematic.Add(da);
            }
            Thread.Sleep(2000);
        }

        if (StopCheckingToken.IsCancellationRequested) return;
        if (problematic.Count > 0 && problematic.Count != count)
        {
            Debug.WriteLine("New problematic");
            await CheckAccounts(problematic, problematic.Count);
        }
        else
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                System.Windows.Forms.MessageBox.Show("Проверка аккаунтов завершена\n" +
                    $"Невалидных аккаунтов: {problematic.Count}\n" +
                    $"{string.Join(", ", problematic.ConvertAll(p => p.Login).ToArray())}");
            });
        }
    }
}
