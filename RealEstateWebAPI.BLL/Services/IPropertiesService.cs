using RealEstateWebAPI.BLL.DTO;

namespace RealEstateWebAPI.BLL.Services
{
    public interface IPropertiesService
    {
        Task<IEnumerable<PropertyDTO>> GetAllPropertiesAsync();
        Task<PropertyDTO> GetPropertyByIdAsync(int propertyId);
        Task<int> AddPropertyAsync(PropertyDTO propertyDTO);
        Task UpdatePropertyAsync(int propertyId, PropertyDTO propertyDTO);
        Task DeletePropertyAsync(int propertyId);
    }
}