using EcommerceDemo.Globals;
using EcommerceDemo.Models;
using EcommerceDemo.Singeltons;

namespace EcommerceDemo.Pages;

public partial class StorePage : ContentPage
{
    
    private readonly DatabaseService _databaseConnection = DatabaseService.Instance;
    private List<Product> _allProducts = [];
    public StorePage()
    {
        InitializeComponent();
        ProfileRequest.SelectedIndexChanged += ProfileChoice;
        
       Cart.Clicked += GoToCart;
    }

    private void ProfileChoice(object? sender, EventArgs e) {

        switch(ProfileRequest.SelectedItem) {
            case "Profile":
                Navigation.PushAsync(new ProfilePage());
                break;
            case "Log Out":
                _databaseConnection.UserLog = null;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                  Shell.Current.Navigation.PopToRootAsync();
                });
                break;
        }
        
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        ProfileRequest.SelectedIndex = 0;
        
        _allProducts = await _databaseConnection.GetProducts();
        
        foreach(Product item in _allProducts) {
            decimal finalPrice = (decimal)item.Price;
            if(item.DiscountPercent >= 1) {
                finalPrice *= 1 - (decimal)item.DiscountPercent / 100m;
                item.HasDiscount = true;
                
                item.DisplayOriginalPrice = CurrencyHelper.GetConversion((decimal)item.Price, _databaseConnection.UserLog.BalanceType);
                item.DisplayPrice = CurrencyHelper.GetConversion(finalPrice, _databaseConnection.UserLog.BalanceType);
                item.DiscountBadge = item.DiscountPercent + "%";
                continue;
            }
            
            item.DisplayOriginalPrice = "";
            item.HasDiscount = false;
            item.DisplayPrice = CurrencyHelper.GetConversion(finalPrice, _databaseConnection.UserLog.BalanceType);
        }
        ItemsCollection.ItemsSource = _allProducts;
    }


    private async void OnProductTapped(object? sender, TappedEventArgs e) {

        if(e.Parameter is Product product) {
            await Navigation.PushAsync(new ProductView(product));
        }
           
        
    }

    private void OnPointerEnter(object? sender, PointerEventArgs e) {
        if(sender is Border border) {
            border.Stroke = Colors.Black;
        }
    }

    private void OnPointerExited(object? sender, PointerEventArgs e) {
        if(sender is Border border) {
            border.Stroke = Colors.LightGray;
        }
    }

    private void OnSearchBarChanged(object? sender, TextChangedEventArgs e) {
        
        var query = e.NewTextValue?.Trim();

        if (string.IsNullOrWhiteSpace(query))
        {
            ItemsCollection.ItemsSource = _allProducts;
            return;
        }

        List<Product> filtered = _allProducts.Where(p => (p.Title != null && p.Title.Contains(query, StringComparison.OrdinalIgnoreCase)) || (p.Description!=null && p.Description.Contains(query,StringComparison.OrdinalIgnoreCase)) || (p.Brand != null && p.Brand.Contains(query,StringComparison.OrdinalIgnoreCase)) || (p.Category != null && p.Category.Contains(query,StringComparison.OrdinalIgnoreCase))).ToList();

        ItemsCollection.ItemsSource = filtered;



    }

    private void GoToCart(object? sender, EventArgs eventArgs) {

        Navigation.PushAsync(new CartPage());

    }

}