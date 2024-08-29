using System.Text.Json.Serialization;

namespace Core.Entites
{
    public class ShoppingCart
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("items")]
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}