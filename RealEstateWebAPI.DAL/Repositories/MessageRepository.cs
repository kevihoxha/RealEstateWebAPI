﻿using Microsoft.EntityFrameworkCore;
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
        private DbSet<Message> Messages => _dbContext.Messages;

        public MessageRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        /// <summary>
        /// Merr te gjithe Message asinkronisht.
        /// </summary>
        /// <returns>Nje koleksion Messages </returns>
        public async Task<IEnumerable<Message>> GetAllMessagesByUserAsync(int authenticatedUserId)
        {
            return await Messages.Where(m => m.Property.UserId == authenticatedUserId).ToListAsync();
        }

        /// <summary>
        /// Merr  Messages sipas propertyID asinkronisht.
        /// </summary>
        /// <returns>Nje koleksion Messages ne baze te propertyId </returns>
        public async Task<IEnumerable<Message>> GetMessagesForPropertyAsync(int propertyId)
        { 
            return await Messages
                    .Where(m => m.PropertyId == propertyId)
                    .ToListAsync();
        }

        /// <summary>
        /// Krijon nje message asinkronisht.
        /// </summary>
        public async Task SendMessageAsync(Message message)
        {
            Messages.Add(message);
            await _dbContext.SaveChangesAsync();
        }

    }
}
