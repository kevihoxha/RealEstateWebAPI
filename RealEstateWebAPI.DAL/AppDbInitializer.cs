using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RealEstateWebAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.DAL
{

    public static  class AppDbInitializer { 

        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();


            SeedAdminAndAgentUsers(context);
        }

        private static void SeedAdminAndAgentUsers(AppDbContext context)
        {

            if (context.Users.Any(u => u.UserName == "admin"))
                return; 

            var adminRole = context.Roles.SingleOrDefault(r => r.Name == "admin");
            if (adminRole == null)
            {
                adminRole = new Role
                {
                    Name = "admin",
                    UniqueIdentifier = Guid.NewGuid()
                };
                context.Roles.Add(adminRole);
                context.SaveChanges(); 
            }

            var agentRole = context.Roles.SingleOrDefault(r => r.Name == "agent");
            if (agentRole == null)
            {
                agentRole = new Role
                {
                    Name = "agent",
                    UniqueIdentifier = Guid.NewGuid()
                };
                context.Roles.Add(agentRole);
                context.SaveChanges(); 
            }

           /* var adminUser = new User
            {
                UserName = "admin",
                PasswordHash = HashPasword
                PasswordSalt = "adminpasswordsalt",
                Email = "admin@example.com",
                RoleId = adminRole.RoleId
            };
            context.Users.Add(adminUser);
            context.SaveChanges();*/
        }

    }
    }

