using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.ActionFilters;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.BLL.Services;
using RealEstateWebAPI.Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace RealEstateWebAPI.Controllers
{
    [ApiController]
    [Route("properties")]
    public class PropertyController : BaseController
    {
        private readonly IPropertiesService _propertyService;
        private readonly ILogger<PropertyController> _logger;

        public PropertyController(ILogger<PropertyController> logger, IPropertiesService propertyService) : base(logger)
        {

            _logger = logger;
            _propertyService = propertyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetAllProperties()
        {
            return await HandleAsync<IEnumerable<PropertyDTO>>(async () =>
            {
                var properties = await _propertyService.GetAllPropertiesAsync();
                return Ok(properties);
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyDTO>> GetPropertyById(int id)
        {
            return await HandleAsync<PropertyDTO>(async () =>
            {
                var property = await _propertyService.GetPropertyByIdAsync(id);
                return Ok(property);
            });
        }

        [HttpPost("create")]
        public async Task<ActionResult<int>> AddProperty(PropertyDTO propertyDTO)
        {
            return await HandleAsync<int>(async () =>
            {
                var propertyId = await _propertyService.AddPropertyAsync(propertyDTO);
                    return Ok(propertyId);
            });
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateProperty(int id, PropertyDTO propertyDTO)
        {
            return await HandleAsync(async () =>
            {
                await _propertyService.UpdatePropertyAsync(id, propertyDTO);
            });
        }

        [HttpDelete("delete/{id}")]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult> DeleteProperty(int id)
        {
            return await HandleAsync(async () =>
            {
                await _propertyService.DeletePropertyAsync(id);
            });
        }
        [HttpGet("search/{location}")]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetAllPropertiesByLocationAsync(string location)
        {
            return await HandleAsync<IEnumerable<PropertyDTO>>(async () =>
            {
                var property = await _propertyService.GetAllPropertiesByLocationAsync(location);
                return Ok(property);
            });
        }
    }
}


