using System.Text.Json;
using System.Text.Json.Serialization;
using SQLite;

namespace EcommerceDemo.Models {

public class ProductFetching {
	public List<Product> Products { get; set; }
}

[Table("ProductData")]
public class Product {


	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Category { get; set; }
	public float Price { get; set; }
	[JsonPropertyName("discountPercentage")]
	public float DiscountPercent { get; set; }
	public float Rating { get; set; }
	public int Stock { get; set; }
	public string Brand { get; set; }
	public string Thumbnail { get; set; }
	public string TagsJson { get; set; }
	public string ImagesJson { get; set; }

	[Ignore]
	public List<string> Tags
	{
		get => string.IsNullOrEmpty(TagsJson) ? new() : JsonSerializer.Deserialize<List<string>>(TagsJson);
		set => TagsJson = JsonSerializer.Serialize(value);
	}

	[Ignore]
	public List<string> Images
	{
		get => string.IsNullOrEmpty(ImagesJson) ? new() : JsonSerializer.Deserialize<List<string>>(ImagesJson);
		set => ImagesJson = JsonSerializer.Serialize(value);
	}
}

}