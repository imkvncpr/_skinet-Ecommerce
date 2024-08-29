using Core.Entites;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers 
{
    public class CartController : BaseAPIController 
    {
        private readonly ICartService _cartService;
        private readonly JsonSerializerOptions _jsonOptions;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        [HttpGet]
        public async Task<ActionResult<ShoppingCart>> GetCartById(string id)
        {
            var cart = await _cartService.GetCartAsync(id);
            return Ok(cart ?? new ShoppingCart { Id = id, Items = new List<CartItem>() });
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingCart>> UpdateCart([FromBody] JsonElement cartData)
        {
            try
            {
                ShoppingCart? cart = null;
                if (cartData.TryGetProperty("ms", out var msProperty))
                {
                    // Handle the case where the cart data is nested under 'ms'
                    var items = JsonSerializer.Deserialize<List<CartItem>>(msProperty.GetRawText(), _jsonOptions);
                    cart = new ShoppingCart { Id = Guid.NewGuid().ToString(), Items = items ?? new List<CartItem>() };
                }
                else
                {
                    // Try to deserialize directly to ShoppingCart
                    cart = JsonSerializer.Deserialize<ShoppingCart>(cartData.GetRawText(), _jsonOptions);
                }

                if (cart == null)
                {
                    return BadRequest("Invalid cart data");
                }

                // Ensure Id is set
                if (string.IsNullOrEmpty(cart.Id))
                {
                    cart.Id = Guid.NewGuid().ToString();
                }

                var updatedCart = await _cartService.SetCartAsync(cart);
                
                if (updatedCart == null) return BadRequest("Problem updating the cart");
                return Ok(updatedCart);
            }
            catch (JsonException ex)
            {
                return BadRequest($"Error parsing cart data: {ex.Message}");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteCart(string id)
        {
            var result = await _cartService.DeleteCartAsync(id);
            if (!result) return BadRequest("Problem deleting the cart");
            return Ok();
        }
    }
}