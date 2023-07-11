using Microsoft.EntityFrameworkCore;
using RealEstateWebAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.DAL.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _dbContext;

        public MessageRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Message>> GetMessagesForPropertyAsync(int propertyId)
        {
            return await _dbContext.Messages
                .Where(m => m.PropertyId == propertyId)
                .ToListAsync();
        }

        public async Task SendMessageAsync(Message message)
        {
            _dbContext.Messages.Add(message);
            await _dbContext.SaveChangesAsync();
        }

    }
}
