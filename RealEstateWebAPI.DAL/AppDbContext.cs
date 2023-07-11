using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateWebAPI.DAL.Entities;

namespace RealEstateWebAPI.DAL
{
    public class AppDbContext : DbContext
    {
        public User GetUserWithRole(int userId)
        {
            return Users.Include(u => u.Role)
                        .FirstOrDefault(u => u.UserId == userId);
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Property>()
                .Property(e => e.Price)
                .HasPrecision(18, 4);
            modelBuilder.Entity<Transaction>()
                .Property(e => e.SalePrice)
                .HasPrecision(18, 4);
        }
        public DbSet<Property> Properties { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role>Roles { get; set; }
        public DbSet<Transaction> Transactions { get; set; }


    }
}
