using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Extensions;
using EcommerceDemo.Globals;
using EcommerceDemo.Models;
using EcommerceDemo.Singeltons;

namespace EcommerceDemo.Pages;

public partial class CheckoutPage : ContentPage {
	
	private DatabaseService _databaseConnection = DatabaseService.Instance;

	private List<CartItem> _cartItems =[];
	public CheckoutPage() {
		InitializeComponent();
		InitializeData();
	}
	
	private async void InitializeData() {
		
		
		List<CartItem> items = await _databaseConnection.GetCartItems();
		ItemsList.ItemsSource = items;
		_cartItems = items;
		float totalCartAmount = MathF.Ceiling(items.Sum(item => item.TotalPrice)*100)/100;

		CartTotalAmount.Text = "Total amount: "+ CurrencyHelper.GetConversion((decimal)totalCartAmount, _databaseConnection.UserLog.BalanceType);
	}
	private async void OnBackButton(object? sender, EventArgs eventArgs) {

		await Navigation.PopAsync();
	}

	private async void OnCheckoutButton(object? sender, EventArgs eventArgs) {
		CheckOutStatus status = await _databaseConnection.Checkout(_cartItems);
		if (status == CheckOutStatus.Failed)
		{
			await this.ShowPopupAsync(new PopupRequest("Failed try again later!"));
		}else if (status == CheckOutStatus.InsufficientFunds)
		{
			await this.ShowPopupAsync(new PopupRequest("Insufficient funds!"));

		}
		IReadOnlyList<Page> navStack = Navigation.NavigationStack;
		if(navStack.Count < 2) {
			return;
		}

		var cartPage = navStack[^2];
		Navigation.RemovePage(cartPage);
		await this.ShowPopupAsync(new PopupRequest("Successfully Paid!"));
		await Navigation.PopAsync();

	}
}


