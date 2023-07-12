using RealEstateWebAPI.DAL.Entities;

namespace RealEstateWebAPI.DAL.Repositories
{
        public interface IMessageRepository
        {
            Task<IEnumerable<Message>> GetMessagesForPropertyAsync(int propertyId);
            Task SendMessageAsync(Message message);
            Task<IEnumerable<Message>> GetAllMessagesByUserAsync(int authenticatedUserId);
        }
    }
