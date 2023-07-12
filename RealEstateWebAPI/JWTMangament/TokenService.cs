using Microsoft.IdentityModel.Tokens;
using RealEstateWebAPI.BLL.DTO;
using RealEstateWebAPI.DAL;
using RealEstateWebAPI.DAL.Entities;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RealEstateWebAPI.JWTMangament
{
    public class TokenService
    {

        private const int ExpirationMinutes = 30;
        public string CreateToken(User user, AppDbContext context)
        {
            var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
            var token = CreateJwtToken(
                CreateClaims(user, context),
                CreateSigningCredentials(),
                expiration
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
            DateTime expiration) =>
            new(
                "apiWithAuthBackend",
                "apiWithAuthBackend",
                claims,
                expires: expiration,
                signingCredentials: credentials
            );

        private List<Claim> CreateClaims(User user,AppDbContext context)
        {
            try
            {
                var userWithRole = context.GetUserWithRole(user.UserId);
                var claims = new List<Claim>
                {
                new Claim(JwtRegisteredClaimNames.Sub,  user.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                };
                if (userWithRole.Role != null && !string.IsNullOrEmpty(userWithRole.Role.Name))
                {
                    claims.Add(new Claim(ClaimTypes.Role, userWithRole.Role.Name));
                }



                return claims;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("!SomethingSecret!12345abcd ergijnewr orwjngkjebwrkg reijbgkewbgrkhwberg")
                ),
                SecurityAlgorithms.HmacSha256
            );
        }
    }
}
