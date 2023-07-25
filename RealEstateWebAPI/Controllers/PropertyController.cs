/*using Microsoft.AspNetCore.Authorization;*/
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.ActionFilters;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.BLL.Services;
using RealEstateWebAPI.Common;
using RealEstateWebAPI.Middleware;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RealEstateWebAPI.Controllers
{
    [ApiController]
    [Route("properties")]
    public class PropertyController : BaseController
    {
        private readonly IPropertiesService _propertyService;

        public PropertyController(IPropertiesService propertyService)
        {
            _propertyService = propertyService;
        }
        /// <summary>
        ///aksesi ne kete endpoint eshte anonymous ,  merr te gjithe properties
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetAllProperties()
        {
            return await HandleAsync<IEnumerable<PropertyDTO>>(async () =>
            {
                var properties = await _propertyService.GetAllPropertiesAsync();
                return Ok(properties);
            });
        }
        /// <summary>
        ///pasi kalon authentikimin nga middleware ,  merr nje porperty me id specifike
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<PropertyDTO>> GetPropertyById(int id)
        {
            return await HandleAsync<PropertyDTO>(async () =>
            {
                var property = await _propertyService.GetPropertyByIdAsync(id);
                return Ok(property);
            });
        }
        /// <summary>
        ///pasi kalon authentikimin nga middleware ,  krijon nje porperty e cila do te marre id e agjentit qe po e krijon
        /// </summary>
        [HttpPost("create")]
        public async Task<ActionResult<int>> AddProperty(PropertyDTO propertyDTO)
        {
            int userId = GetAuthenticatedUserId();

            return await HandleAsync<int>(async () =>
            {
                var propertyId = await _propertyService.AddPropertyAsync(propertyDTO, userId);
                return Ok(propertyId);
            });
        }
        /// <summary>
        ///pasi kalon authentikimin nga middleware ,  modifikon nje porperty , nese eshte i njejti agjent qe e ka krijuar ate
        /// </summary>
        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateProperty(int id, PropertyDTO propertyDTO)
        {
            int userId = GetAuthenticatedUserId();
            return await HandleAsync(async () =>
            {
                await _propertyService.UpdatePropertyAsync(id, propertyDTO, userId);
            });
        }

        /// <summary>
        ///pasi kalon authentikimin nga middleware ,  fshin nje porperty , nese eshte i njejti agjent qe e ka krijuar ate
        /// </summary>
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteProperty(int id, PropertyDTO propertyDTO)
        {
            int userId = GetAuthenticatedUserId();
            return await HandleAsync(async () =>
            {
                await _propertyService.UpdatePropertyAsync(id, propertyDTO, userId);
            });
        }
        /// <summary>
        ///aksesi ne kete endpoint eshte anonymous ,  merr te gjithe properties sipas lokacionit
        /// </summary>
        [HttpGet("search/{location}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetAllPropertiesByLocationAsync(string location)
        {
            return await HandleAsync<IEnumerable<PropertyDTO>>(async () =>
            {
                var property = await _propertyService.GetAllPropertiesByLocationAsync(location);
                return Ok(property);
            });
        }
        /// <summary>
        /// kjo metode merr nga claims ID e user te authentikuar ne moment , dhe e kthen ate ne int
        /// </summary>
        private int GetAuthenticatedUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim);
        }
    }
}


