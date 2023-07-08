using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.DAL.Entities;
using RealEstateWebAPI.DAL.Repositories;

namespace RealEstateWebAPI.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RealEstateWebAPI.BLL.DTO;
    using RealEstateWebAPI.BLL.Services;
    using RealEstateWebAPI.JWTMangament;

    [ApiController]
   
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IUsersService _usersService;
        public AuthController(TokenService tokenService,IUsersService usersService)
        {
            _tokenService = tokenService;
            _usersService = usersService;
        }
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var managedUser = await _usersService.GetUserByUserNameAsync(request.Username);
            if (managedUser == null)
            {
                return BadRequest("Bad credentials");
            }
            if (request.Password!=managedUser.PasswordHash)
            {
                return BadRequest("Bad credentials");
            }

            var accessToken = _tokenService.CreateToken(managedUser);
            return Ok(new AuthResponse
            {
                Username = managedUser.UserName,
                Email = managedUser.Email,
                Token = accessToken,
            });
        }
    }
}