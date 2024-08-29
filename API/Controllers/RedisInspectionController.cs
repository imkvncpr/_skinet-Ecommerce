using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RedisInspectionController : BaseAPIController
    {
        private readonly RedisCartInspection _redisInspection;

        public RedisInspectionController(RedisCartInspection redisInspection)
        {
            _redisInspection = redisInspection;
        }

        [HttpGet("health")]
        public ActionResult CheckRedisHealth()
        {
            var (isHealthy, message) = _redisInspection.CheckRedisHealth();
            if (isHealthy)
                return Ok(new { IsHealthy = isHealthy, Message = message });
            else
                return StatusCode(503, new { IsHealthy = isHealthy, Message = message });
        }

        [HttpGet("cart/{cartId}")]
        public ActionResult InspectCart(string cartId)
        {
            var (success, result) = _redisInspection.InspectRedisCart(cartId);
            if (success)
                return Ok(result);
            else
                return NotFound(result);
        }
    }
}