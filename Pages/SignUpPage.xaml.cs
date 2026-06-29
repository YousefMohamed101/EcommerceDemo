namespace EcommerceDemo.Pages;

public partial class SignUpPage : ContentPage
{
	public SignUpPage()
	{
		InitializeComponent();
	}
    private async void Back(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

}