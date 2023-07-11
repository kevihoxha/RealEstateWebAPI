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
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Check if the user is authenticated
/*            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }*/

            if (!context.HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.Name && c.Value == "admin"))
            {
                context.Result = new ForbidResult();
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Perform any necessary actions after the action is executed
        }

    }
}

