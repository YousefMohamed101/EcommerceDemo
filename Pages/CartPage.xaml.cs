using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommerceDemo.Globals;
using EcommerceDemo.Models;
using EcommerceDemo.Singeltons;

namespace EcommerceDemo.Pages;

public partial class CartPage : ContentPage {
	
	private DatabaseService _databaseConnection = DatabaseService.Instance;
	private List<Product> ItemsInCart;
	public CartPage() {
		InitializeComponent();
		InitializeData();

		CheckoutButton.Clicked += (sender, args) => {Navigation.PushAsync(new CheckoutPage());};
	}

	public async void InitializeData() {
		
		
		List<CartItem> items = await _databaseConnection.GetCartItems();
		foreach(CartItem item in items) {
			item.DisplayPrice = CurrencyHelper.GetConversion((decimal)item.Price, _databaseConnection.UserLog.BalanceType);
			item.DisplayTotalPrice = CurrencyHelper.GetConversion((decimal)item.TotalPrice, _databaseConnection.UserLog.BalanceType);
		}
		ItemsShow.ItemsSource = items;
	}


	private async void RemoveItemFromCart(object sender, EventArgs e) {
		if(sender is not ImageButton { CommandParameter: CartItem cartItem }) {
			return;
		}

		await _databaseConnection.RemoveFromCart(cartItem);
		InitializeData();

	}
	private async void OnBackButton(object? sender, EventArgs eventArgs) {

		await Navigation.PopAsync();
	}

	public async void OnIncreaseProductAmount(object? sender, EventArgs eventArgs) {

		if(sender is not Button { CommandParameter: CartItem item }) {
			return;
		}
		item.Count = Math.Abs(item.Count- (item.Count + 1));
		await _databaseConnection.AddToCart(item);
		InitializeData();
		
	}
	public async void OnDecreaseProductAmount(object? sender, EventArgs eventArgs) {
		if(sender is not Button { CommandParameter: CartItem item }) {
			return;
		}

		if (item.Count <= 1)
		{
			return;
		}
		item.Count -= (item.Count + 1);
		await _databaseConnection.AddToCart(item);
		InitializeData();
		
	}
}