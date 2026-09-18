using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
namespace EcommerceDemo.Globals;

public partial class PopupRequest : Popup {
	public PopupRequest(string message) {
		InitializeComponent();
		Message.Text = message;
	}

	public void OnClose(object sender, EventArgs e) {
		CloseAsync();
	}
}

