using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.Common.ErrorHandeling;
using Serilog;

namespace RealEstateWebAPI.Controllers
{
    public class BaseController : ControllerBase
    {

        protected async Task<ActionResult<T>> HandleAsync<T>(Func<Task<ActionResult<T>>> action)
        {
            try
            {
                return await action();
            }
            catch (CustomException ex)
            {
                throw;
            }
        }

        protected async Task<ActionResult> HandleAsync(Func<Task> action)
        {
            try
            {
                await action();
                return NoContent();
            }
            catch (CustomException ex)
            {
                throw;
            }
        }
    }
}
