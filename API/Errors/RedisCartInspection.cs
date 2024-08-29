
using System.Text.Json;
using StackExchange.Redis;

namespace API.Errors
{
    public class RedisCartInspection(string redisUrl)
    
        {
    private readonly ConnectionMultiplexer _redis = ConnectionMultiplexer.Connect(redisUrl);

        public (bool success, string result) InspectRedisCart(string cartId)
    {
        try
        {
            var db = _redis.GetDatabase();
            var cartData = db.HashGetAll($"cart:{cartId}");

            if (cartData.Length == 0)
            {
                return (false, $"No data found for cart ID: {cartId}");
            }

            var cartDictionary = cartData.ToDictionary(
                kvp => kvp.Name.ToString(),
                kvp => kvp.Value.ToString()
            );

            string prettyCart = JsonSerializer.Serialize(cartDictionary, new JsonSerializerOptions { WriteIndented = true });
            return (true, $"Cart data for ID {cartId}:\n{prettyCart}");
        }
        catch (Exception ex)
        {
            return (false, $"An error occurred while inspecting cart: {ex.Message}");
        }
    }

    public (bool isHealthy, string message) CheckRedisHealth()
    {
        try
        {
            var db = _redis.GetDatabase();

            // Ping Redis to check connectivity
            db.Ping();

            // Try a simple set and get operation
            db.StringSet("test_key", "test_value");
            var value = db.StringGet("test_key");

            if (value != "test_value")
            {
                return (false, "Set/Get operation failed");
            }

            // Clean up
            db.KeyDelete("test_key");

            return (true, "Redis is healthy");
        }
        catch (RedisConnectionException)
        {
            return (false, "Could not connect to Redis");
        }
        catch (Exception ex)
        {
            return (false, $"An error occurred: {ex.Message}");
        }
    }
}

    }
