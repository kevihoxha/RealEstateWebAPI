using Microsoft.EntityFrameworkCore;
using RealEstateWebAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.DAL.Repositories
{

    public class PropertyRepository : IPropertyRepository
    {
        private readonly AppDbContext _dbContext;
        private DbSet<Property> Properties => _dbContext.Properties;

        public PropertyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Merr te gjithe Properties asinkronisht.
        /// </summary>
        /// <returns>Nje koleksion Properties pervec atyre qe jane softDelete.</returns>
        public async Task<IEnumerable<Property>> GetAllPropertiesAsync()
        {
            return await Properties.Where(p => !p.IsDeleted).ToListAsync();
        }

        /// <summary>
        /// Merr nje property asinkronisht ne baze te id .
        /// </summary>
        /// <returns>Nje Property pervec atij qe eshte softDeleted.</returns>
        public async Task<Property> GetPropertyByIdAsync(int propertyId)
        {
            return await Properties.FirstOrDefaultAsync(p => p.PropertyId == propertyId && !p.IsDeleted);
        }

        /// <summary>
        /// Merr nje property ne baze te lokacionit asinkronisht.
        /// </summary>
        /// <returns>>Nje Property</returns>
        public async Task<IEnumerable<Property>> GetPropertyByLocationAsync(string location)
        {
            return await Properties.Where(p => p.Location.Contains(location) && !p.IsDeleted).ToListAsync();
        }

        /// <summary>
        /// Shton nje Property asinkronisht.
        /// </summary>
        public async Task AddPropertyAsync(Property property)
        {
            Properties.Add(property);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Modifikon nje Property asinkronisht.
        /// </summary>
        public async Task UpdatePropertyAsync(Property property)
        {
            _dbContext.Entry(property).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Fshin nje Property asinkronisht.
        /// </summary>
        public async Task DeletePropertyAsync(int id)
        {
            var property = await Properties.FindAsync(id);
            if (property != null)
            {
                property.IsDeleted = true;
                property.DeletedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Merr nje property asinkronisht ne baze te UserId .
        /// </summary>
        /// <returns>Nje  koleksion Property pervec atij qe eshte softDeleted.</returns>
        public async Task<IEnumerable<Property>> GetPropertiesByUserIdAsync(int userId)
        {
            return await Properties.Where(p => p.UserId == userId && !p.IsDeleted).ToListAsync();
        }
    }
}
