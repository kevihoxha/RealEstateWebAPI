using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RealEstateWebAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.DAL
{
    /// <summary>
    /// Kjo klase do te inicializoje databazen e applikacionit
    /// </summary>
    public static class AppDbInitializer
    {
        /// <summary>
        /// Inicializon databazen dhe sigurohet nese ajo eshte ndezur apo jo 
        /// </summary>
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();


            SeedAdminAndAgentUsers(context);
        }
        /// <summary>
        /// Ben Seed rolet e admin dhe agent ne databaze nese ato nuk ekzitojne.
        /// </summary>
        private static async Task SeedAdminAndAgentUsers(AppDbContext context)
        {
            if (await context.Users.AnyAsync(u => u.UserName == "admin"))
                return;

            var adminRole = await context.Roles.SingleOrDefaultAsync(r => r.Name == "admin");
            if (adminRole == null)
            {
                adminRole = new Role
                {
                    Name = "admin",
                    UniqueIdentifier = Guid.NewGuid()
                };
                await context.Roles.AddAsync(adminRole);
                await context.SaveChangesAsync();
            }

            var agentRole = await context.Roles.SingleOrDefaultAsync(r => r.Name == "agent");
            if (agentRole == null)
            {
                agentRole = new Role
                {
                    Name = "agent",
                    UniqueIdentifier = Guid.NewGuid()
                };
                await context.Roles.AddAsync(agentRole);
                await context.SaveChangesAsync();
            }
        }

    }
}

