using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RealEstateWebAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.DAL
{

    public static class AppDbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // Add your data seeding logic here
            SeedAdminAndAgentUsers(context);
        }

        private static void SeedAdminAndAgentUsers(AppDbContext context)
        {
            // Check if the admin user already exists
            if (context.Users.Any(u => u.UserName == "admin"))
                return; // Admin user already seeded

            // Create the admin role if it doesn't exist
            var adminRole = context.Roles.SingleOrDefault(r => r.Name == "admin");
            if (adminRole == null)
            {
                adminRole = new Role
                {
                    Name = "admin",
                    UniqueIdentifier = Guid.NewGuid()
                };
                context.Roles.Add(adminRole);
            }

            // Create the agent role if it doesn't exist
            var agentRole = context.Roles.SingleOrDefault(r => r.Name == "agent");
            if (agentRole == null)
            {
                agentRole = new Role
                {
                    Name = "agent",
                    UniqueIdentifier = Guid.NewGuid()
                };
                context.Roles.Add(agentRole);
            }

            // Create the admin user
            var adminUser = new User
            {
                UserName = "admin",
                PasswordHash = "adminpasswordhash",
                PasswordSalt = "adminpasswordsalt",
                Email = "admin@example.com",
                RoleId = adminRole.RoleId
            };
            context.Users.Add(adminUser);

           /* // Create the agent user
            var agentUser = new User
            {
                UserName = "agent",
                PasswordHash = "agentpasswordhash",
                PasswordSalt = "agentpasswordsalt",
                Email = "agent@example.com",
                RoleId = agentRole.RoleId
            };
            context.Users.Add(agentUser);*/

            context.SaveChanges();

        }
    }
}
