using AutoMapper;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.Common.ErrorHandeling;
using RealEstateWebAPI.DAL.Entities;
using RealEstateWebAPI.DAL.Repositories;
using Serilog;

namespace RealEstateWebAPI.BLL.Services
{
    public class PropertiesService : IPropertiesService
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

            var properties = await _propertyRepository.GetAllPropertiesAsync();
            if (properties != null)
            {
                Log.Information("Got all Properties");
                return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
            }
            throw new CustomException("Couldnt get Properties");
        }
        /// <summary>
        /// Merr nje property asinkronisht ne baze te id .
        /// </summary>
        /// <returns>Nje Property pervec atij qe eshte softDeleted.</returns>
        public async Task<PropertyDTO> GetPropertyByIdAsync(int propertyId)
        {
            var property = await _propertyRepository.GetPropertyByIdAsync(propertyId);
            if (property != null)
            {
                Log.Information($"Got property with ID: {propertyId}");
                return _mapper.Map<PropertyDTO>(property);
            }
            throw new CustomException("Property not found");
        }
        /// <summary>
        /// Shton nje Property asinkronisht.
        /// </summary>
        ///
        public async Task<int> AddPropertyAsync(PropertyDTO propertyDTO, int userId)
        {
            var property = _mapper.Map<Property>(propertyDTO);
            property.UserId = userId; // Assign the user ID to the property
            await _propertyRepository.AddPropertyAsync(property);
            Log.Information("Property added succesfully");
            return property.PropertyId;
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
            var property = await _propertyRepository.GetPropertyByIdAsync(propertyId);
            if (property != null)
            {
                // Update the property details
                _mapper.Map(propertyDTO, property);
                property.UserId = userId; // Assign the user ID to the property
                await _propertyRepository.UpdatePropertyAsync(property);
                Log.Information("Property  updated succesfully");
            }
            throw new CustomException("Property not found");
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
            var propertyToDelte = await _propertyRepository.GetPropertyByIdAsync(propertyId);
            if (propertyToDelte != null)
            {
                await _propertyRepository.DeletePropertyAsync(propertyId);
                Log.Information($"Property with id {propertyToDelte.PropertyId} got deleted");
            }

            throw new CustomException("Property not found");
        }
        /// <summary>
        /// Merr nje property ne baze te lokacionit asinkronisht.
        /// </summary>
        /// <returns>>Nje Property</returns>
        public async Task<IEnumerable<PropertyDTO>> GetAllPropertiesByLocationAsync(string location)
        {

            var properties = await _propertyRepository.GetPropertyByLocationAsync(location);
            if (properties != null)
            {
                Log.Information("Got all Properties");
                return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
            }
            throw new CustomException("Couldnt get properties");
        }
    }
}

