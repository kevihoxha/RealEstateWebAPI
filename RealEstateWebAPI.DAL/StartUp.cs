using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateWebAPI.DAL.Repositories;

namespace RealEstateWebAPI.DAL
{
        public static class StartUp
        {
            public static void RegisterDalServices(this IServiceCollection services, IConfiguration configuration)
            {

                services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("RealEstateWebAPIDb")));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
        }
        }
}