using EcommerceDemo.Pages;

namespace EcommerceDemo
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SignInPage), typeof(SignInPage));
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(WelcomLoading), typeof(WelcomLoading));
            Routing.RegisterRoute(nameof(StorePage), typeof(StorePage));
        }

    }
}
