using RealEstateWebAPI.BLL.DTO;

namespace RealEstateWebAPI.BLL.Services
{
    public interface IPropertiesService
    {
        Task<IEnumerable<PropertyDTO>> GetAllPropertiesAsync();
        Task<PropertyDTO> GetPropertyByIdAsync(int propertyId);
        Task<int> AddPropertyAsync(PropertyDTO propertyDTO,int userId);
        Task UpdatePropertyAsync(int propertyId, PropertyDTO propertyDTO,int userId);
        Task DeletePropertyAsync(int propertyId, int userId);
        Task<IEnumerable<PropertyDTO>> GetAllPropertiesByLocationAsync(string location);
    }
}