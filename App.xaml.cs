using EcommerceDemo.Singeltons;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceDemo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override async void OnStart()
        {
            await DatabaseService.CreateAsync();
        }
    }
}