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
    public class PropertiesService : IPropertiesService
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public PropertiesService(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertyDTO>> GetAllPropertiesAsync()
        {
            var properties = await _propertyRepository.GetAllPropertiesAsync();
            return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
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
    }
}