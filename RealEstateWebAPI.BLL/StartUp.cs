using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateWebAPI.BLL.Services;
using RealEstateWebAPI.DAL;
using AutoMapper;
namespace RealEstateWebAPI.BLL
{
    public static class Startup
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration config)
        {
            services.RegisterDalServices(config);
            services.AddScoped<IPropertiesService, PropertiesService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddAutoMapper(typeof(Startup));
        }
    }
}