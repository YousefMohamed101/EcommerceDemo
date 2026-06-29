using EcommerceDemo.Pages;
using System.Diagnostics;
using System.Windows.Input;

namespace EcommerceDemo
{
    public partial class MainPage : ContentPage
    {
        public ICommand SignInCommand { get; }
        public ICommand SignUpCommand { get; }

        public MainPage()
        {
            InitializeComponent();
            SignInCommand = new Command(async () =>
            await Shell.Current.GoToAsync(nameof(SignInPage)));

            SignUpCommand = new Command(async () =>
                await Shell.Current.GoToAsync(nameof(SignUpPage)));
        }

       

        private async void SignInButton_Clicked(object? sender, EventArgs e)
        {
            Debug.WriteLine("pressed Sign In");
            await SignInTravel();

        }
        private async void SignUpButton_Clicked(object? sender, EventArgs e)
        {
            Debug.WriteLine("pressed Sign Up");
            await SignUpTravel();

        }
        private async Task SignInTravel()
        {
            
            await Shell.Current.GoToAsync(nameof(SignInPage));

        }
        private async Task SignUpTravel()
        {

            await Shell.Current.GoToAsync(nameof(SignUpPage));

        }
    }
}
