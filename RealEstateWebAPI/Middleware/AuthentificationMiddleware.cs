using System.Net;

namespace RealEstateWebAPI.Middleware
{


    public class AuthenticationMiddleware : IMiddleware
    {
        
            public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // Check if the request contains a token in the header
       if (context.Request.Path.Equals("/login", StringComparison.OrdinalIgnoreCase))
    {
        // Allow anonymous access to the login endpoint
        await next(context);
        return;
    }

    // Check if the request contains a token in the header
    if (!context.Request.Headers.ContainsKey("Authorization"))
    {
        context.Response.StatusCode = 401; // Unauthorized
        context.Response.ContentType = "text/plain";
        await context.Response.WriteAsync("You are not authorized to be here.");
        return;
    }

    // Verify the token and perform additional authentication/authorization checks
    // Your token verification logic goes here

    // If the token is valid and the user is authorized, proceed to the next middleware
    await next(context);
}
    } }