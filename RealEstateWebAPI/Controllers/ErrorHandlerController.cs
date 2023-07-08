using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.Common.ErrorHandeling;

namespace RealEstateWebAPI.Controllers
{
    [ApiController]
    [Route("api/{controller}")]
    public class ErrorHandlerController : ControllerBase
    {
        [HttpGet]
        public IActionResult HandleError(NotFoundException ex)
        {
            return Problem("An error occurred: " + ex.Message, statusCode: 500);
        }
    }
}
