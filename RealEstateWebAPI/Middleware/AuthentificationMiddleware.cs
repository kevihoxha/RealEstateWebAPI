using Microsoft.IdentityModel.Tokens;
using RealEstateWebAPI.JWTMangament;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace RealEstateWebAPI.Middleware
{

    /// <summary>
    /// Middleware for handling authentication.
    /// </summary>
    public class AuthenticationMiddleware : IMiddleware
    {
        /// <summary>
        /// Ndez middleware.
        /// </summary>
        /// <param name="context"> HTTP context.</param>
        /// <param name="next">Delegati i middleware te rradhes</param>
        /// <returns>Nje Task qe tregon se middleware ka mbaruar funksionin</returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            /// <summary>
            /// Kontrollon nese lejohet aksesi si anonymous
            /// </summary>
            if (HasAllowAnonymousAttribute(context))

            {
                await next(context);
                return;
            }
            /// <summary>
            /// Kontrollon nese ka nje token ne header
            /// </summary>
            string token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Missing token.");
                return;
            }
            /// <summary>
            /// Validimi i token te marre nga header
            /// </summary>
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
        /// <summary>
        /// Kontrollon nese ai endpoint eshte i dekoruar me AllowAnonymous attribute
        /// </summary>
        private bool HasAllowAnonymousAttribute(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var attributes = endpoint.Metadata.GetOrderedMetadata<AllowAnonymousAttribute>();
                return attributes.Any();
            }
            else
            {
                context.Response.WriteAsync("You are not authenticated");
                return false;
            }
        }
    }
}