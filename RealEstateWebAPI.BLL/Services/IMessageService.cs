using RealEstateWebAPI.BLL.DTO;

namespace RealEstateWebAPI.BLL.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDTO>> GetMessagesForPropertyAsync(int propertyId);
        Task SendMessageAsync(MessageDTO messageDTO);
        Task<IEnumerable<MessageDTO>> GetAllMessagesByUserAsync(int authenticatedUserId);
    }
}