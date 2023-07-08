using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.BLL.Services;
using System.Collections.Generic;
using RealEstateWebAPI.Common.ErrorHandeling;

namespace RealEstateWebAPI.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : BaseController 
    {
        private readonly IUsersService _userService;
        private readonly ILogger<PropertyController> _logger;

        public UserController(ILogger<PropertyController> logger,IUsersService userService):base(logger) 
        { 
        
            _userService = userService;
             _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            return await HandleAsync<IEnumerable<UserDTO>>(async () =>
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok (users);
            });
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            throw new Common.ErrorHandeling.NotFoundException("missing user");
        
            return await HandleAsync<UserDTO>(async () =>
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                throw new Common.ErrorHandeling.NotFoundException("missing user");
            }
            return user;
            });
        }

        [HttpPost("create")]
        public async Task<ActionResult<int>> AddUser(UserDTO userDTO)
        {
            return await HandleAsync<int>(async () =>
            {
                var userId = await _userService.AddUserAsync(userDTO);
                return userId;
            });
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult> UpdateUser(int id, UserDTO userDTO)
        {
            return await HandleAsync(async () =>
            {
               await  _userService.UpdateUserAsync(id, userDTO);
            });
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            return await HandleAsync(async () =>
            {
                 await _userService.DeleteUserAsync(id);
            });
        }
       
    }
}
