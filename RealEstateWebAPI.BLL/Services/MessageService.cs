using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.Common.ErrorHandeling;
using RealEstateWebAPI.DAL.Entities;
using RealEstateWebAPI.DAL.Repositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL.Services
{
    public class MessageService : ExceptionHandling, IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public MessageService(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Merr te gjithe Message asinkronisht.
        /// </summary>
        /// <returns>Nje koleksion Messages </returns>
        public async Task<IEnumerable<MessageDTO>> GetAllMessagesByUser(int authenticatedUserId)
        {
            return await HandleAsync(async () =>
            {
                var messages = await _messageRepository.GetAllMessagesByUserAsync(authenticatedUserId);
                if (messages != null)
                {
                    Log.Information($"User {authenticatedUserId} got all his messages ");
                    return _mapper.Map<IEnumerable<MessageDTO>>(messages);
                }
                throw new CustomException($"User {authenticatedUserId} failed to get all his messages ");
            });
        }

        /// <summary>
        /// Merr  Messages sipas propertyID asinkronisht.
        /// </summary>
        /// <returns>Nje koleksion Messages ne baze te propertyId </returns>
        public async Task<IEnumerable<MessageDTO>> GetMessagesForPropertyAsync(int propertyId)
        {
            return await HandleAsync(async () =>
            {
                var messages = await _messageRepository.GetMessagesForPropertyAsync(propertyId);
                if (messages.Any())
                {
                    Log.Information($"Got all messages for property {propertyId}");
                    return _mapper.Map<IEnumerable<MessageDTO>>(messages);
                }
                throw new CustomException($"There are no messages for property {propertyId}");
            });
        }
        /// <summary>
        /// Krijon nje message asinkronisht.
        /// </summary>
        public async Task SendMessageAsync(MessageDTO messageDTO)
        {
            await HandleAsync(async () =>
            {
                var message = _mapper.Map<Message>(messageDTO);
                await _messageRepository.SendMessageAsync(message);
                Log.Information("Message sent");
            });
        }
    }
}
