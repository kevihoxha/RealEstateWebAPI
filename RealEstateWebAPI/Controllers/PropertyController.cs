using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.BLL.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstateWebAPI.Controllers
{
    [ApiController]
    [Route("api/properties")]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertiesService _propertyService;

        public PropertyController(IPropertiesService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetAllProperties()
        {
            var properties = await _propertyService.GetAllPropertiesAsync();
            return Ok(properties);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyDTO>> GetPropertyById(int id)
        {
            var property = await _propertyService.GetPropertyByIdAsync(id);
            if (property == null)
            {
                return NotFound();
            }
            return Ok(property);
        }

        [HttpPost]
        public async Task<ActionResult<int>> AddProperty(PropertyDTO propertyDTO)
        {
            try
            {
                var propertyId = await _propertyService.AddPropertyAsync(propertyDTO);
                return Ok(propertyId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProperty(int id, PropertyDTO propertyDTO)
        {
            try
            {
                await _propertyService.UpdatePropertyAsync(id, propertyDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProperty(int id)
        {
            try
            {
                await _propertyService.DeletePropertyAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

