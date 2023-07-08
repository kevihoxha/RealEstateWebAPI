using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RealEstateWebAPI.Common.ErrorHandeling;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace RealEstateWebAPI.Middleware
{
    public class GlobalErrorHandlingMiddleware : IMiddleware
    { 
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);

            }catch(NotFoundException ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Custom Error: " + ex.Message);
            }
        }
    }
}
