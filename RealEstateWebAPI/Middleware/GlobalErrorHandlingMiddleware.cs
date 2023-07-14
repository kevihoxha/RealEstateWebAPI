using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RealEstateWebAPI.Common.ErrorHandeling;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace RealEstateWebAPI.Middleware
{
    /// <summary>
    /// Middleware for handling global errors.
    /// </summary>
    public class GlobalErrorHandlingMiddleware : IMiddleware
    {
        /// <summary>
        /// Ndez kete middleware
        /// </summary>
        /// <param name="context"> HTTP context.</param>
        /// <param name="next">Delegati i middleware te rradhes</param>
        /// <returns>Nje Task qe tregon se middleware ka mbaruar funksionin</returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);

            }
            catch (CustomException ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Custom Error: " + ex.Message);
                Log.Error(ex.Message);  
            }
        }
    }
}
