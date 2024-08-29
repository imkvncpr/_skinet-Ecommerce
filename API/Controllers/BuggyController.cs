using API.DTOs;
using Core.Entites;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseAPIController
    {
        [HttpGet("Unauthorized")]
        public IActionResult GetUnauthorized()
        {
            return Unauthorized("You are not authorized to access this resource");
        }

        [HttpGet("BadRequest")]
        public IActionResult GetBadRequest()
        {
            return BadRequest("This was a bad request");
        }
        
        [HttpGet("NotFound")]
        public IActionResult GetNotFound()
        {
            return NotFound("Resource was not found");
        }

        [HttpGet("InternalError")]
        public IActionResult GetInternalError()
        {
            throw new Exception("This is a test exception");
        }
        
        [HttpPost("ValidationError")]
        public IActionResult GetValidationError(CreateProductDTO product)
        {
            return Ok();
        }
    }
}