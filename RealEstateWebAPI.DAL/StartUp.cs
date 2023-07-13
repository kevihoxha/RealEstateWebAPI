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
        /// <summary>
        /// Regjistrimi i serviseve te shtreses DAL , marrja e connectionstring , si dhe Seed ne Database
        /// </summary>
        public static void RegisterDalServices(this IServiceCollection services, IConfiguration configuration)
        {
            /// <summary>
            /// Regjistron DAL Services ne dependency injection container
            /// </summary>
            /// <param name="services">The <see cref="IServiceCollection"/> Kryen regjistrimin e serviseve</param>
            /// <param name="configuration">The <see cref="IConfiguration"/> permban cilesimet e aplikacionit</param>
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("RealEstateWebAPIDb")));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
        }
        /// <summary>
        /// Ben Seed ne Database kur programi nis te ekzekutohet
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> nje instance per IApplicationBuilder</param>
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

