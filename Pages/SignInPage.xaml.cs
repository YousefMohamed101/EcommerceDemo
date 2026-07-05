using EcommerceDemo.Models;
using EcommerceDemo.Singeltons;

namespace EcommerceDemo.Pages;

public partial class SignInPage : ContentPage
{


    private readonly DatabaseService _databaseConnection;

	public SignInPage()
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

    private async void FindUser(object sender, EventArgs e)
    {

        User user = new User()
        {
            Name = UsernameEntry.Text,
            Password = PasswordEntry.Text
           
        };

        bool succeed =await _databaseConnection.IsUserExist(user);

        if (succeed)
        {
            _databaseConnection.UserLog = user;
            await Shell.Current.GoToAsync(nameof(WelcomLoading));
        }


    }
}