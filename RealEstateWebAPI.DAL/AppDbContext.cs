using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstateWebAPI.DAL.Entities;

namespace RealEstateWebAPI.DAL
{
    /// <summary>
    /// Perfaqeson context e databazes se applikacionit
    /// </summary>
    public class AppDbContext : DbContext
    {/// <summary>
     /// Inicializon nje instance te re pa parametra te : <see cref="AppDbContext"/> class.
     /// </summary>
        public AppDbContext() : base()
        {
        }
        /// <summary>
        /// Merr nje user dhe rolin e tij ne baze te UserId .
        /// </summary>
        /// <param name="userId">Id e Userit qe do te merret</param>
        /// <returns>User me rolin e tij.</returns>
        public User GetUserWithRole(int userId)
        {
            return Users.Include(u => u.Role)
                        .FirstOrDefault(u => u.UserId == userId);
        }
        /// <summary>
        /// Inicializon nje instance te re me parametra te <see cref="AppDbContext"/> class .
        /// </summary>
        /// <param name="options">Opsionet per konfigurimin e context</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        /// <summary>
        /// Ben konfigurimet e modeleve per Database Context
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Property>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Property>()
            .HasOne(p => p.Transaction)
            .WithOne(t => t.Property)
            .HasForeignKey<Transaction>(t => t.PropertyId)
            .IsRequired();

            modelBuilder.Entity<Property>()
                .Property(e => e.Price)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Transaction>()
                .Property(e => e.SalePrice)
                .HasPrecision(18, 4);

        }
        /// <summary>
        /// Gets or sets the properties DbSet.
        /// </summary>
        public DbSet<Property> Properties { get; set; }
        /// <summary>
        /// Gets or sets the users DbSet.
        /// </summary>
        public DbSet<User> Users { get; set; }
        /// <summary>
        /// Gets or sets the roles DbSet.
        /// </summary>
        public DbSet<Role> Roles { get; set; }
        /// <summary>
        /// Gets or sets the transactions DbSet.
        /// </summary>
        public DbSet<Transaction> Transactions { get; set; }
        /// <summary>
        /// Gets or sets the messages DbSet.
        /// </summary>
        public DbSet<Message> Messages { get; set; }


    }
}
