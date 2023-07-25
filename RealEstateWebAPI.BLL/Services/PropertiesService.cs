using AutoMapper;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.Common.ErrorHandeling;
using RealEstateWebAPI.DAL.Entities;
using RealEstateWebAPI.DAL.Repositories;
using Serilog;

namespace RealEstateWebAPI.BLL.Services
{
    public class PropertiesService : ExceptionHandling, IPropertiesService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public PropertiesService(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Merr te gjithe Properties asinkronisht.
        /// </summary>
        /// <returns>Nje koleksion Properties pervec atyre qe jane softDelete.</returns>
        public async Task<IEnumerable<PropertyDTO>> GetAllPropertiesAsync()
        {
            return await HandleAsync(async () =>
            {
                var properties = await _propertyRepository.GetAllPropertiesAsync();
                if (properties != null)
                {
                    Log.Information($"Retrieved {properties.Count()} properties.");
                    return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
                }
                throw new CustomException("Could not get properties.");
            });
        }
        /// <summary>
        /// Merr nje property asinkronisht ne baze te id .
        /// </summary>
        /// <returns>Nje Property pervec atij qe eshte softDeleted.</returns>
        public async Task<PropertyDTO> GetPropertyByIdAsync(int propertyId)
        {
            return await HandleAsync(async () =>
            {
                var property = await _propertyRepository.GetPropertyByIdAsync(propertyId);
                if (property != null)
                {
                    Log.Information($"Retrieved property with ID: {propertyId}");
                    return _mapper.Map<PropertyDTO>(property);
                }
                Log.Warning($"Property with ID: {propertyId} not found.");
                throw new CustomException("Property not found");
            });
        }
        /// <summary>
        /// Shton nje Property asinkronisht.
        /// </summary>
        ///
        public async Task<int> AddPropertyAsync(PropertyDTO propertyDTO, int userId)
        {
            return await HandleAsync(async () =>
            {
                var property = _mapper.Map<Property>(propertyDTO);
                AssignUserIdToProperty(property, userId);
                await _propertyRepository.AddPropertyAsync(property);
                Log.Information($"Property with ID {property.PropertyId} added successfully by User ID {userId}.");
                return property.PropertyId;
            });
        }
        /// <summary>
        /// Modifikon nje Property asinkronisht.
        /// </summary>
        public async Task UpdatePropertyAsync(int propertyId, PropertyDTO propertyDTO, int userId)
        {
            await HandleAsync(async () =>
        {
            var property = await _propertyRepository.GetPropertyByIdAsync(propertyId);
            if (property != null)
            {
                if (property.UserId != userId)
                {
                    Log.Warning($"User with ID {userId} is not authorized to update Property with ID {propertyId}.");
                    throw new CustomException($"User with ID {userId} is not authorized to update Property with ID {propertyId}.");
                }

                
                propertyDTO.PropertyId = propertyId;
                _mapper.Map(propertyDTO, property);
                AssignUserIdToProperty(property, userId);
                await _propertyRepository.UpdatePropertyAsync(property);
                Log.Information($"Property with ID {propertyId} updated successfully.");
            }
            else
            {
                throw new CustomException($"Property with ID {propertyId} not found.");
            }
        });
        }
        /// <summary>
        /// Fshin nje Property asinkronisht.
        /// </summary>
        public async Task DeletePropertyAsync(int propertyId,int userId)
        {
            await HandleAsync(async () =>
            {
                var propertyToDelete = await _propertyRepository.GetPropertyByIdAsync(propertyId);
                if (propertyToDelete != null)
                {
                    if (propertyToDelete.UserId != userId)
                    {
                        Log.Warning($"User with ID {userId} is not authorized to delete Property with ID {propertyId}.");
                        throw new CustomException($"User with ID {userId} is not authorized to delete Property with ID {propertyId}.");
                    }

                    await _propertyRepository.DeletePropertyAsync(propertyId);
                    Log.Information($"Property with ID {propertyId} deleted successfully.");
                }
                else
                {
                    throw new CustomException($"Property with ID {propertyId} not found.");
                }
            });
        }
        /// <summary>
        /// Merr nje property ne baze te lokacionit asinkronisht.
        /// </summary>
        /// <returns>>Nje Property</returns>
        public async Task<IEnumerable<PropertyDTO>> GetAllPropertiesByLocationAsync(string location)
        {
            return await HandleAsync(async () =>
            {
                var properties = await _propertyRepository.GetPropertyByLocationAsync(location);
                if (properties.Any())
                {
                    Log.Information($"Retrieved {properties.Count()} properties for location: {location}");
                    return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
                }
                Log.Warning($"No properties found for location: {location}");
                throw new CustomException("No properties found for the specified location.");
            });
        }
        private void AssignUserIdToProperty(Property property, int userId)
        {
            property.UserId = userId;
        }
    }
}

