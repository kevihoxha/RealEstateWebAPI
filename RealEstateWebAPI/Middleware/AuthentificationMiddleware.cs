using System.Net;

namespace RealEstateWebAPI.Middleware
{


    public class AuthenticationMiddleware : IMiddleware
    {
        
            public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

       if (context.Request.Path.Equals("/login", StringComparison.OrdinalIgnoreCase)|| context.Request.Path.Equals("/properties", StringComparison.OrdinalIgnoreCase) || context.Request.Path.StartsWithSegments("/properties/search", StringComparison.OrdinalIgnoreCase))
    {

        await next(context);
        return;
    }

            if (!context.Request.Headers.ContainsKey("Authorization"))
    {
        context.Response.StatusCode = 401; 
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("You are not authorized to be here.");
        return;
    }

    await next(context);
}
    } }