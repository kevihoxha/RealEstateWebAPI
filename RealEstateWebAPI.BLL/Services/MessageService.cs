using AutoMapper;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.DAL.Entities;
using RealEstateWebAPI.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL.Services
{
    public class MessageService : IMessageService
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
            var messages = await _messageRepository.GetAllMessagesByUserAsync(authenticatedUserId);
            return _mapper.Map<IEnumerable<MessageDTO>>(messages);
        }
        /// <summary>
        /// Merr  Messages sipas propertyID asinkronisht.
        /// </summary>
        /// <returns>Nje koleksion Messages ne baze te propertyId </returns>
        public async Task<IEnumerable<MessageDTO>> GetMessagesForPropertyAsync(int propertyId)
        {
            var messages = await _messageRepository.GetMessagesForPropertyAsync(propertyId);
            return _mapper.Map<IEnumerable<MessageDTO>>(messages);
        }
        /// <summary>
        /// Krijon nje message asinkronisht.
        /// </summary>
        public async Task SendMessageAsync(MessageDTO messageDTO)
        {
            var message = _mapper.Map<Message>(messageDTO);
            await _messageRepository.SendMessageAsync(message);
        }
    }
}
