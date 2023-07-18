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
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }
    }
}
