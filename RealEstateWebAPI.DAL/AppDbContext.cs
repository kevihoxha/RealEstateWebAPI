using Microsoft.EntityFrameworkCore;
using RealEstateWebAPI.DAL.Entities;

namespace RealEstateWebAPI.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Property> Properties { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
