using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Extensions;
using EcommerceDemo.Globals;
using EcommerceDemo.Models;
using EcommerceDemo.Singeltons;

namespace EcommerceDemo.Pages;

public partial class ProductView : ContentPage {
	
	private int _totalImages = 0;
	private int _amountRequested;
	private Product _product;
	private DatabaseService _databaseConnection = DatabaseService.Instance;
	
	public ProductView(Product product) {
		InitializeComponent(); 
		
		AmountLabel.Text = _amountRequested.ToString();
		_product = product;
		ProductTitle.Text =  product.Title;
		ImageList.ItemsSource = product.Images;

		if(product.DiscountPercent >= 1) {
			Discount.IsVisible = true;
			DiscountBadge.IsVisible = true;
			Discount.Text = CurrencyHelper.GetConversion((decimal)product.Price, DatabaseService.Instance.UserLog.BalanceType);
			
			decimal discountedPrice = (decimal)(product.Price * (1 - product.DiscountPercent / 100));
			Price.Text = CurrencyHelper.GetConversion(discountedPrice, DatabaseService.Instance.UserLog.BalanceType);
			DiscountBadge.Text = product.DiscountPercent + "%";

		} else {
			Discount.IsVisible = false;
			DiscountBadge.IsVisible = false;
			Price.Text = CurrencyHelper.GetConversion((decimal)product.Price, DatabaseService.Instance.UserLog.BalanceType);

		}
		
		ProductDescription.Text = product.Description;
		

		_totalImages = product.Images.Count;
		
		IncreaseAmount.Clicked += (object? sender, EventArgs e)=> UpdateRequestAmount(1);
		DecreaseAmount.Clicked += (object? sender, EventArgs e)=> UpdateRequestAmount(-1);
		
		AmountLabel.TextChanged += (object? sender, TextChangedEventArgs  e) => SetRequestAmount(e.NewTextValue);
	}
	
	

	private void OnNextClicked(object? sender, EventArgs e) {
		if (ImageList.Position > 0)
		{
			ImageList.Position -= 1;
		}
	}

	private void OnPreviousClicked(object? sender, EventArgs e) {
		if (ImageList.Position < _totalImages-1)
		{
			ImageList.Position += 1;
		}
	}

	private async void OnAddToCartClicked(object? sender, EventArgs e) {
		if(_amountRequested <= 0) {
			await this.ShowPopupAsync(new PopupRequest("Please add an amount"));
			return;
		}
		
		CartItem cartItem = new CartItem();
		cartItem.ItemId = _product.Id;
		cartItem.UserId = _databaseConnection.UserLog.Id;
		cartItem.Count = _amountRequested;
		_ = _databaseConnection.AddToCart(cartItem);
		await this.ShowPopupAsync(new PopupRequest("Successfully added to cart"));
		await Navigation.PopAsync();

	}

	public void UpdateRequestAmount(int i) {
		if(_amountRequested + i < 0 || _totalImages + i > _product.Stock) {
			return;
		}
		
		_amountRequested += i;
		AmountLabel.Text = _amountRequested.ToString();
		
		
	}

	public void SetRequestAmount(string s) {

		_amountRequested = int.Parse(s);
		

	}
	
	private async void OnBackButton(object? sender, EventArgs eventArgs) {

		await Navigation.PopAsync();
	}


	
}