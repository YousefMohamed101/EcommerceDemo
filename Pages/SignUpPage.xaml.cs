using EcommerceDemo.Globals;
using EcommerceDemo.Models;
using EcommerceDemo.Singeltons;
using System.Diagnostics;
using System.Net.Mail;
using CommunityToolkit.Maui.Extensions;

namespace EcommerceDemo.Pages;

public partial class SignUpPage : ContentPage
{

    private readonly DatabaseService _databaseConnection;

    public SignUpPage()
    {
        InitializeComponent();
        _databaseConnection = DatabaseService.Instance;

    }
    private async void Back(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private void ShowPassword(object sender, EventArgs e)
    {

        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;

    }

    private async void RegisterUser(object sender, EventArgs e)
    {

        if(!MailAddress.TryCreate(EmailEntry.Text, out MailAddress? email)) {
            await this.ShowPopupAsync(new PopupRequest("Incorrect Email"));
            return;
        }
        
        
        User user = new User()
        {
            Name = UsernameEntry.Text,
            Email = email.ToString(),
            Password = PasswordEntry.Text,
            BalanceType = GetMoneyType((string)CurrencyPicker.SelectedItem),
            IsAdmin = AdminCheckBox.IsChecked

        };

        bool succeed = await _databaseConnection.RegisterUser(user);
        if (!succeed)
        {
            await this.ShowPopupAsync(new PopupRequest("Failed to registered try again!"));
            return;
        }
        await this.ShowPopupAsync(new PopupRequest("successfully registered"));
        await Shell.Current.GoToAsync("..");
    }


    private MoneyType GetMoneyType(string s)
    {

        return s switch
        {
            "Euros €" => MoneyType.Euros,
            "Yen ¥" => MoneyType.Yen,
            _ => MoneyType.Dollars,
        };
    }



}