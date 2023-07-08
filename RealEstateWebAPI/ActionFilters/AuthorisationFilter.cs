using Microsoft.AspNetCore.Mvc.Filters;
using System.Drawing.Text;

namespace RealEstateWebAPI.ActionFilters
{
    public class AuthorisationFilter:ActionFilterAttribute
    {
        private string _roleRequired { get; set; }
        public AuthorisationFilter(string roleRequired)
        {
            _roleRequired = roleRequired ?? "Anonymous";
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
        /*    var x = context.ActionDescriptor.AttributeRouteInfo;*/
            if (_roleRequired == "Admin")
            {

            }
            base.OnActionExecuting(context);
            
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
    }
}
