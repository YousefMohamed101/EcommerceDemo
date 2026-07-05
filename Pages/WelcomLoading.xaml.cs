using EcommerceDemo.Singeltons;
using Microsoft.Maui.Animations;
using System.Diagnostics;

namespace EcommerceDemo.Pages;

public partial class WelcomLoading : ContentPage
{
    private readonly DatabaseService _databaseConnection;

    public WelcomLoading()
    {
        InitializeComponent();
        _databaseConnection = DatabaseService.Instance;

        GreetingLabel.Text = $"Welcome,{_databaseConnection.UserLog.Name}";
        GreetingLabel.Opacity = 0;
        StartAnimationPlay();

    }

    private async void StartAnimationPlay()
    {
        bool finished = await GreetingLabel.FadeToAsync(1, 2000);

        if (!finished)
        {

            await Task.Delay(1000);
            await Shell.Current.GoToAsync(nameof(StorePage));

        }
    }
}