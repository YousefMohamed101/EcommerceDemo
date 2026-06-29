using EcommerceDemo.Models;
using EcommerceDemo.Singeltons;

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

        User user = new User()
        {
            Name = UsernameEntry.Text,
            Email = EmailEntry.Text,
            Password = PasswordEntry.Text,
            //BalanceType = MoneyType.USD,

        };

        await _databaseConnection.RegisterUser(user.Name, user.Email, user.Password);
    }

}