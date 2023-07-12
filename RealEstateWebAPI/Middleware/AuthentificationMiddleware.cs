using Microsoft.IdentityModel.Tokens;
using RealEstateWebAPI.JWTMangament;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace RealEstateWebAPI.Middleware
{


    public class AuthenticationMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {

            if (/*context.Request.Path.Equals("/login", StringComparison.OrdinalIgnoreCase)
                || context.Request.Path.Equals("/properties", StringComparison.OrdinalIgnoreCase)
                || context.Request.Path.StartsWithSegments("/properties/search", StringComparison.OrdinalIgnoreCase
                )*/ HasAllowAnonymousAttribute(context))

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
            string token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Invalid token.");
                return;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecret = "!SomethingSecret!12345abcd ergijnewr orwjngkjebwrkg reijbgkewbgrkhwberg";
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                context.User = claimsPrincipal;
            }
            catch (Exception)
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Invalid token.");
                return;
            }

            await next(context);
        }

        private bool HasAllowAnonymousAttribute(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var attributes = endpoint.Metadata.GetOrderedMetadata<AllowAnonymousAttribute>();
                return attributes.Any();
            }
            return false;
        }
    }
    } 