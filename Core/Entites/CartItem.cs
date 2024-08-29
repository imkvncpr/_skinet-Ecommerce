using System.Text.Json.Serialization;

public class CartItem
{
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("productName")]
    public required string ProductName { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("pictureUrl")]
    public required string PictureUrl { get; set; }
    
    [JsonPropertyName("brand")]
    public required string Brand { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }
}