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
                    Log.Information("Got all Properties");
                    return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
                }
                throw new CustomException("Couldnt get Properties");
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
                    Log.Information($"Got property with ID: {propertyId}");
                    return _mapper.Map<PropertyDTO>(property);
                }
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
                property.UserId = userId; // Assign the user ID to the property
                await _propertyRepository.AddPropertyAsync(property);
                Log.Information("Property added succesfully");
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
                // Update the property details
                propertyDTO.PropertyId = propertyId;
                _mapper.Map(propertyDTO, property);
                property.UserId = userId; // Assign the user ID to the property
                await _propertyRepository.UpdatePropertyAsync(property);
                Log.Information("Property  updated succesfully");
            }
            else
            {
                throw new CustomException("User does not exists");
            }
        });
        }
        /// <summary>
        /// Fshin nje Property asinkronisht.
        /// </summary>
        public async Task DeletePropertyAsync(int propertyId)
        {
            await HandleAsync(async () =>
            {
                var propertyToDelte = await _propertyRepository.GetPropertyByIdAsync(propertyId);
                if (propertyToDelte != null)
                {
                    await _propertyRepository.DeletePropertyAsync(propertyId);
                    Log.Information($"Property with id {propertyToDelte.PropertyId} got deleted");
                }
                else
                {
                    throw new CustomException("Property not found");
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
                    Log.Information("Got all Properties");
                    return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
                }
                throw new CustomException("Couldnt get properties");
            });
        }
    }
}

