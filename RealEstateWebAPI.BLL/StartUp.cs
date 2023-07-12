using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateWebAPI.BLL.Services;
using RealEstateWebAPI.DAL;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using RealEstateWebAPI.DAL.Entities;

namespace RealEstateWebAPI.BLL
{
    public static class Startup
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration config)
        {
            services.RegisterDalServices(config);
            services.AddScoped<IPropertiesService, PropertiesService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IMessageService, MessageService>();

            services.AddAutoMapper(typeof(Startup));
            

        }
    }
}