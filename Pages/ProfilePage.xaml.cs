using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
		if(_userCopy.ImagePath != null) {
			PfButton.Source = _userCopy.ImagePath;
		}
		if(_userCopy.IsAdmin) {
			Admin.IsVisible = true;
		}
		AllowUserEdit.Clicked += (sender, e) => AllowEdit(0);
		AllowEmailEdit.Clicked += (sender, e) => AllowEdit(1);

		UserName.Completed += (sender, e) => UpdateUserName(UserName.Text);
		Email.Completed += (sender, e) => UpdateEmail(Email.Text);

	}

	private async void UpdatPfImage(object? sender, EventArgs eventArgs) {

		var imageFilter = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>> {
				{ DevicePlatform.WinUI, new[] { ".png", ".jpg", ".jpeg" } }
		});
		var options = new PickOptions {
				PickerTitle = "Choose an Image", FileTypes = imageFilter
		};
		
		FileResult? result = await FilePicker.PickAsync(options);
		if(result != null) {
			_userCopy.ImagePath = result.FullPath;
			_databaseService.UpdateUser(_userCopy);
		}



	}

	private void AllowEdit(int i) {

		if(i == 0) {
			UserName.IsEnabled = !UserName.IsEnabled;
			if(UserName.IsEnabled) {
				UserName.BackgroundColor = Colors.LightGray;
				
			} else {
				UserName.BackgroundColor = Colors.Gray;
			}
		}else if(i == 1) {
			Email.IsEnabled = !Email.IsEnabled;
			if(Email.IsEnabled) {
				Email.BackgroundColor = Colors.LightGray;
				
			} else {
				Email.BackgroundColor = Colors.Gray;
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
}