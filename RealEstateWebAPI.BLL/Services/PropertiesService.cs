using AutoMapper;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.DAL.Entities;
using RealEstateWebAPI.DAL.Repositories;


namespace RealEstateWebAPI.BLL.Services
{
    public class PropertiesService : IPropertiesService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public PropertiesService(Microsoft.Extensions.Logging.ILogger<PropertiesService> logger, IPropertyRepository propertyRepository, IMapper mapper)
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
            try
            {
                var properties = await _propertyRepository.GetAllPropertiesAsync();

                return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Merr nje property asinkronisht ne baze te id .
        /// </summary>
        /// <returns>Nje Property pervec atij qe eshte softDeleted.</returns>
        public async Task<PropertyDTO> GetPropertyByIdAsync(int propertyId)
        {
            try
            {
                var property = await _propertyRepository.GetPropertyByIdAsync(propertyId);
                return _mapper.Map<PropertyDTO>(property);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Shton nje Property asinkronisht.
        /// </summary>
        /// 

        public async Task<int> AddPropertyAsync(PropertyDTO propertyDTO, int userId)
        {
            try
            {
                var property = _mapper.Map<Property>(propertyDTO);
                property.UserId = userId; // Assign the user ID to the property

                await _propertyRepository.AddPropertyAsync(property);
                return property.PropertyId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /* public async Task<int> AddPropertyAsync(PropertyDTO propertyDTO)
         {
             try
             {
                 var property = _mapper.Map<Property>(propertyDTO);
                 await _propertyRepository.AddPropertyAsync(property);
                 return property.PropertyId;
             }
             catch (Exception ex)
             {
                 throw;
             }
         }*/
        /// <summary>
        /// Modifikon nje Property asinkronisht.
        /// </summary>
        public async Task UpdatePropertyAsync(int propertyId, PropertyDTO propertyDTO, int userId)
        {
            try
            {
                var property = await _propertyRepository.GetPropertyByIdAsync(propertyId);
                if (property != null)
                {
                    // Update the property details
                    _mapper.Map(propertyDTO, property);
                    property.UserId = userId; // Assign the user ID to the property

                    await _propertyRepository.UpdatePropertyAsync(property);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /* public async Task UpdatePropertyAsync(int propertyId, PropertyDTO propertyDTO)
         {
             try
             {
                 var property = await _propertyRepository.GetPropertyByIdAsync(propertyId);
                 if (property != null)
                 {
                     _mapper.Map(propertyDTO, property);
                     await _propertyRepository.UpdatePropertyAsync(property);
                 }
             }
             catch (Exception ex)
             {
                 throw;
             }
         }*/
        /// <summary>
        /// Fshin nje Property asinkronisht.
        /// </summary>
        public async Task DeletePropertyAsync(int propertyId)

        {
            try
            {
                await _propertyRepository.DeletePropertyAsync(propertyId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        /// <summary>
        /// Merr nje property ne baze te lokacionit asinkronisht.
        /// </summary>
        /// <returns>>Nje Property</returns>
        public async Task<IEnumerable<PropertyDTO>> GetAllPropertiesByLocationAsync(string location)
        {
            try
            {
                var properties = await _propertyRepository.GetPropertyByLocationAsync(location);
                return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}