using Microsoft.AspNetCore.Mvc;

namespace RealEstateWebAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly ILogger<BaseController> _logger;

        public BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        protected async Task<ActionResult<T>> HandleAsync<T>(Func<Task<ActionResult<T>>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                /* _logger.LogError(ex.Message);*/
                return BadRequest(ex.Message);
            }
        }

        protected async Task<ActionResult> HandleAsync(Func<Task> action)
        {
            try
            {
                await action();
                return NoContent();
            }
            catch (Exception ex)
            {
               /* _logger.LogError(ex.Message);*/
                return BadRequest(ex.Message);
            }
        }
    }
}
