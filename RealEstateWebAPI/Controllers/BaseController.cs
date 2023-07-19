using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.Common.ErrorHandeling;
using Serilog;

namespace RealEstateWebAPI.Controllers
{
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Trajton nje Action Asinkron qe kthen vlere <see cref="ActionResult{T}"/>.
        /// </summary>
        /// <typeparam name="T">Tipi i rezultatit qe kthen ky action</typeparam>
        /// <param name="action">Aksioni asinkron qe do te ekzekutohet</param>
        /// <returns>Nje <see cref="ActionResult{T}"/> si rezultat i ketij action.</returns>
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
        /// <summary>
        /// Trajton nje Action asinkron qe nuk kthen vlere.
        /// </summary>
        /// <param name="action">Aksionin asinkron qe do te ekzekutohet</param>
        /// <returns>Nje <see cref="ActionResult"/> si rezultat i ketij aksioni</returns>
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
