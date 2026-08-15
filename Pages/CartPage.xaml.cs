using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommerceDemo.Models;
using EcommerceDemo.Singeltons;

namespace EcommerceDemo.Pages;

public partial class CartPage : ContentPage {
	
	private DatabaseService _databaseConnection = DatabaseService.Instance;
	private List<Product> ItemsInCart;
	public CartPage() {
		InitializeComponent();
		InitializeData();
	}

	public async void InitializeData() {
		
		
		ItemsShow.ItemsSource = await _databaseConnection.GetCartItems();
	}


	private async void RemoveItemFromCart(object sender, EventArgs e) {
		if(sender is not ImageButton { CommandParameter: CartItem cartItem }) {
			return;
		}

		await _databaseConnection.RemoveFromCart(cartItem);
		InitializeData();

	}
}