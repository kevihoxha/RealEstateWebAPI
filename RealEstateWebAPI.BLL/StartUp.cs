using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateWebAPI.BLL.Services;
using RealEstateWebAPI.DAL;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Configuration;
using Microsoft.AspNetCore.Identity;
using RealEstateWebAPI.DAL.Entities;
using RealEstateWebAPI.BLL.DTO;

namespace RealEstateWebAPI.BLL
{
    public static class Startup
    {
        /// <summary>
/// Metoda per regjistrimin e serviseve ne dependency injection container
/// </summary>
        public static void RegisterServices(this IServiceCollection services, IConfiguration config)
        {
            services.RegisterDalServices(config);
            services.AddScoped<IPropertiesService, PropertiesService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddSingleton<IEmailService, EmailService>();
            services.Configure<EmailServiceSettings>(config.GetSection("EmailServiceSettings"));
            services.AddAutoMapper(typeof(Startup));


        }
    }
}