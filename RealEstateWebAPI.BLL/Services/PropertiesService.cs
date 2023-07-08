using AutoMapper;
using Microsoft.Extensions.Logging;
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
    public class PropertiesService : IPropertiesService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;
        private readonly Microsoft.Extensions.Logging.ILogger<PropertiesService> _logger; 

        public PropertiesService(Microsoft.Extensions.Logging.ILogger<PropertiesService> logger, IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
            _logger= logger;
        }

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

        public async Task<PropertyDTO> GetPropertyByIdAsync(int propertyId)
        {
            var property = await _propertyRepository.GetPropertyByIdAsync(propertyId);
            return _mapper.Map<PropertyDTO>(property);
        }

        public async Task<int> AddPropertyAsync(PropertyDTO propertyDTO)
        {
            var property = _mapper.Map<Property>(propertyDTO);
            await _propertyRepository.AddPropertyAsync(property);
            return property.PropertyId;
        }

        public async Task UpdatePropertyAsync(int propertyId, PropertyDTO propertyDTO)
        {
            var property = await _propertyRepository.GetPropertyByIdAsync(propertyId);
            if (property != null)
            {
                _mapper.Map(propertyDTO, property);
                await _propertyRepository.UpdatePropertyAsync(property);
            }
        }

        public async Task DeletePropertyAsync(int propertyId)
        {
            await _propertyRepository.DeletePropertyAsync(propertyId);
        }
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