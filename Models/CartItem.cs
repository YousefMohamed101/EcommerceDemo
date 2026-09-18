using System.ComponentModel.DataAnnotations.Schema;
using SQLite;

namespace EcommerceDemo.Models;


[SQLite.Table("CartItems")]
public class CartItem {

	[PrimaryKey,AutoIncrement] public int Id {get; set;}
	public int UserId {get; set;}
	public int ItemId {get; set;}
	public string Name {get; set;}
	public string ImagePath {get; set;}
	public float Price {get; set;}
	public int Count {get; set;}
	public float TotalPrice => Price * Count;
	
	[Ignore] public string DisplayPrice { get; set; }
	[Ignore] public string DisplayTotalPrice { get; set; }
	
}