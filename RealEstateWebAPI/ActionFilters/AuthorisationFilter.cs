using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using RealEstateWebAPI.DAL;
using System.Drawing.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RealEstateWebAPI.ActionFilters
{
    public class AuthorisationFilter : IActionFilter
    {
        private const string RequiredRole = "admin";
        /// <summary>
        /// Thirret perpara se nje metode qe ekzekutohet, performon nje kontroll authorizimi per kete metode.
        /// </summary>
        /// <param name="context">Context per metoden qe do te ekzekutohet</param>
        public void OnActionExecuting(ActionExecutingContext context)
        {

            if (!IsUserAuthorized(context.HttpContext.User))
            {
                context.Result = new ObjectResult("You are not authorized")
                {
                    StatusCode = 403
                };
            }
        }
        /// <summary>
        /// Thirret pasi nje metode ekzekutohet, nuk ben asgje ne kete rast.
        /// </summary>
        /// <param name="context">Context per metoden pas ekzekuimit.</param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
        private bool IsUserAuthorized(ClaimsPrincipal user)
        {
            return user.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == RequiredRole);
        }
    }
}

