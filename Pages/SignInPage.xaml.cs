namespace EcommerceDemo.Pages;

public partial class SignInPage : ContentPage
{
	public SignInPage()
	{
		InitializeComponent();
	}

    private async void Back(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

	private void ShowPassword(object sender, EventArgs e)
	{

        PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
      
    }


}