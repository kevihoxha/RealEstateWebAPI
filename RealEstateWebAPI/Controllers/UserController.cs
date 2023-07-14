using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.BLL.Services;
using System.Collections.Generic;
using RealEstateWebAPI.Common.ErrorHandeling;
using RealEstateWebAPI.ActionFilters;
using Microsoft.AspNetCore.Authorization;

namespace RealEstateWebAPI.Controllers
{

    [ApiController]
    [Route("users")]
    public class UserController : BaseController
    {
        private readonly IUsersService _userService;

        public UserController(IUsersService userService)
        {

            _userService = userService;
        }

        [HttpGet]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            return await HandleAsync<IEnumerable<UserDTO>>(async () =>
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            });
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(AuthorisationFilter))]

        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            /*throw new Common.ErrorHandeling.CustomException("missing user");*/

            return await HandleAsync<UserDTO>(async () =>
            {
                var user = await _userService.GetUserByIdAsync(id);
                return user;
            });
        }

        [HttpPost("create")]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult<int>> AddUser(UserDTO userDTO)
        {
            return await HandleAsync<int>(async () =>
            {
                var userId = await _userService.AddUserAsync(userDTO);
                return userId;
            });
        }

        [HttpPut("update/{id}")]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult> UpdateUser(int id, UserDTO userDTO)
        {
            return await HandleAsync(async () =>
            {
                await _userService.UpdateUserAsync(id, userDTO);
            });
        }

        [HttpDelete("delete/{id}")]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult> DeleteUser(int id)
        {
            return await HandleAsync(async () =>
            {
                await _userService.DeleteUserAsync(id);
            });
        }
        [HttpGet("{userId}/properties")]
        [TypeFilter(typeof(AuthorisationFilter))]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetPropertiesByUserId(int userId)
        {
            return await HandleAsync<IEnumerable<PropertyDTO>>(async () =>
            {
                var properties = await _userService.GetPropertiesByUserIdAsync(userId);
                return Ok(properties);
            });
        }
    }
}
