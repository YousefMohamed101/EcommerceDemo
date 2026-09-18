using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommerceDemo.Globals;
using EcommerceDemo.Models;
using EcommerceDemo.Singeltons;

namespace EcommerceDemo.Pages;

public partial class ProfilePage : ContentPage {
	
	private DatabaseService _databaseService = DatabaseService.Instance;
	private User _userCopy = new User();
	public ProfilePage() {
		InitializeComponent();
		
		
		Admin.IsVisible = false;
		
		_userCopy = _databaseService.UserLog.Clone();
		UserName.Text =  _userCopy.Name;
		Email.Text =  _userCopy.Email;
	
		AddBalance.Text = CurrencyHelper.GetConversion(_userCopy.Balance,_userCopy.BalanceType);
		
		if(!string.IsNullOrEmpty(_userCopy.ImagePath)) {
			PfButton.Source = _userCopy.ImagePath;
		}
		if(_userCopy.IsAdmin) {
			Admin.IsVisible = true;
		}
		AllowUserEdit.Clicked += (sender, e) => AllowEdit(0);
		AllowEmailEdit.Clicked += (sender, e) => AllowEdit(1);
		AllowAmountEdit.Clicked += (sender, e) => AllowEdit(2);

		UserName.Completed += (sender, e) => UpdateUserName(UserName.Text);
		Email.Completed += (sender, e) => UpdateEmail(Email.Text);
		AddBalance.Completed += (object? sender, EventArgs e) => UpdateBlanace(AddBalance.Text);
	}

	private async void UpdatePfImage(object? sender, EventArgs eventArgs) {

		FilePickerFileType imageFilter = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> {
				{ DevicePlatform.WinUI, [".png", ".jpg", ".jpeg"] }
		});
		PickOptions options = new PickOptions {
				PickerTitle = "Choose an Image", FileTypes = imageFilter
		};
		
		FileResult? result = await FilePicker.PickAsync(options);
		if(result == null) {
			return;
		}

		_userCopy.ImagePath = result.FullPath;
		_databaseService.UpdateUser(_userCopy);
		PfButton.Source = _userCopy.ImagePath;



	}

	private void AllowEdit(int i) {

		if(i == 0) {
			UserName.IsEnabled = !UserName.IsEnabled;
			if(UserName.IsEnabled) {
				UserName.BackgroundColor = Colors.LightGray;
				
			} else {
				UserName.BackgroundColor = Colors.Transparent;
			}
		}else if(i == 1) {
			Email.IsEnabled = !Email.IsEnabled;
			if(Email.IsEnabled) {
				Email.BackgroundColor = (Color)Application.Current!.Resources["TextSecondary"];;
				
			} else {
				Email.BackgroundColor = Colors.Transparent;
			}
		}else if(i == 2) {
			AddBalance.IsEnabled = !AddBalance.IsEnabled;
			if(AddBalance.IsEnabled) {
				AddBalance.BackgroundColor = Colors.LightGray;
				
			} else {
				AddBalance.BackgroundColor = Colors.Transparent;
			}
		}
		
		
	}

	private void UpdateUserName(string username) {
		_userCopy.Name =  username;
		_databaseService.UpdateUser(_userCopy);
		AllowEdit(0);
	}
	private void UpdateEmail(string email) {
		_userCopy.Email =  email;
		_databaseService.UpdateUser(_userCopy);
		AllowEdit(1);
	}

	private void UpdateBlanace(string amount) {

		decimal newMoney = decimal.Parse(amount);
		decimal change =  newMoney - _userCopy.Balance;
		
		_userCopy.Balance += change;

		switch(_userCopy.BalanceType) {
			case MoneyType.Dollars:
				AddBalance.Text = $"{_userCopy.Balance}$";
				break;
			case MoneyType.Euros:
				AddBalance.Text = $"{_userCopy.Balance}€";
				break;
			case MoneyType.Yen:
				AddBalance.Text = $"{_userCopy.Balance}¥";
				break;

			default:
				throw new ArgumentOutOfRangeException();
		}
		
		
		_databaseService.UpdateUserWallet(change);
		

	}

	private async void OnBackButton(object? sender, EventArgs eventArgs) {

		await Navigation.PopAsync();
	}
}