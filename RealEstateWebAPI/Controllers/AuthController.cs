
using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.DAL.Entities;
using RealEstateWebAPI.DAL.Repositories;

namespace RealEstateWebAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RealEstateWebAPI.BLL.DTO;
    using RealEstateWebAPI.BLL.Services;
    using RealEstateWebAPI.DAL;
    using RealEstateWebAPI.JWTMangament;
    using RealEstateWebAPI.Middleware;

    [ApiController]

    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IUserRepository _usersService;
        private readonly AppDbContext _appDbContext;
        public AuthController(TokenService tokenService, IUserRepository usersService, AppDbContext context)
        {
            _tokenService = tokenService;
            _usersService = usersService;
            _appDbContext = context;
        }
        /// <summary>
        /// Authentikon nje user me kredencialet e duhura.
        /// </summary>
        /// <param name="request"> <see cref="AuthRequest"/>permban kredencialet per logimin e userit.</param>
        /// <returns>
        /// Nje <see cref="ActionResult{T}"/> te tipit <see cref="AuthResponse"/> permban rezultatin e authentikimit.
        /// Nese authentikimi eshte i suksesshkem kthen  <see cref="AuthResponse"/> me informacionin mbi userin dhe tokenin.
        /// Nese kredencialet jane te gabuara ose nese authentikimi deshton kthen <see cref="BadRequestResult"/>.
        /// </returns>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var managedUser = await _usersService.GetUserByUsernameAsync(request.Username);
            if (managedUser == null)
            {
                return BadRequest("Bad credentials");
            }
            if (PasswordHashing.VerifyPassword(request.Password, managedUser.PasswordHash, managedUser.PasswordSalt))
            {

                var accessToken = _tokenService.CreateToken(managedUser, _appDbContext);
                return Ok(new AuthResponse
                {
                    Username = managedUser.UserName,
                    Email = managedUser.Email,
                    Token = accessToken,
                });
            }
            return new AuthResponse();
        }
    }
}