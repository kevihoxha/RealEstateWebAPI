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

        public PropertyRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Property>> GetAllPropertiesAsync()
        {
            return await _dbContext.Properties.Where(p => !p.IsDeleted).ToListAsync();
        }

        public async Task<Property> GetPropertyByIdAsync(int propertyId)
        {
            return await _dbContext.Properties.FirstOrDefaultAsync(p => p.PropertyId == propertyId && !p.IsDeleted);
        }
        public async Task<IEnumerable<Property>> GetPropertyByLocationAsync(string location)
        {
            return await _dbContext.Properties.Where(p => p.Location.Contains(location)).ToListAsync();
        }



        public async Task AddPropertyAsync(Property property)
        {
            _dbContext.Properties.Add(property);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePropertyAsync(Property property)
        {
            _dbContext.Entry(property).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePropertyAsync(int id)
        {
            var property = await _dbContext.Properties.FindAsync(id);
            if (property != null)
            {
                property.IsDeleted = true;
                property.DeletedAt = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
