using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RealEstateWebAPI.DAL.Repositories;
using Serilog;
using Serilog.Events;

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
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
        }
        public static void SeedData(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<AppDbContext>();


                AppDbInitializer.Initialize(dbContext);
            }
        }
    }
    }

