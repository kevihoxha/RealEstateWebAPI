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
        /// <summary>
        /// Krijon nje JWT token per nje user specifik.
        /// </summary>
        /// <param name="user">user.</param>
        /// <param name="context"> context.</param>
        /// <returns> JWT token.</returns>
        public string CreateToken(User user, AppDbContext context)
        {
            using var dbContext = new AppDbContext();
            //kalkulon kohen se sa minuta do te zgjase tokeni
            var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
            //krijon token
            var token = CreateJwtToken(
                //krijon claims
                CreateClaims(user, context),
                //krijon kredencialet 
                CreateSigningCredentials(),
                // koha e kalkuluar
                expiration
            );
            //merr tokenin
            var tokenHandler = new JwtSecurityTokenHandler();
            //kthen tokenin
            return tokenHandler.WriteToken(token);
        }
        // kjo metode do te ktheje JwtSecurityToken
        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
            DateTime expiration) =>
            new(
                "apiWithAuthBackend",
                "apiWithAuthBackend",
                claims,
                expires: expiration,
                signingCredentials: credentials
            );
        /// <summary>
        /// Krijon nje calims per nje user specifik na baze te token.
        /// </summary>
        /// <param name="user">user.</param>
        /// <param name="context"> context.</param>
        /// <returns>Koleksion te Claims</returns>
        private List<Claim> CreateClaims(User user, AppDbContext context)
        {
            try
            {
                // merr userin me rolin e asajnuar
                var userWithRole = context.GetUserWithRole(user.UserId);
                //krijo nje liste qe mban claims e userit
                var claims = new List<Claim>
                {
                new Claim(JwtRegisteredClaimNames.Sub,  user.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                };
                //kontrollon nese nje perodrues ka nje rol te asenjuar
                if (userWithRole.Role != null && !string.IsNullOrEmpty(userWithRole.Role.Name))
                {
                    //shton nje claim per rolin e userit
                    claims.Add(new Claim(ClaimTypes.Role, userWithRole.Role.Name));
                }


                // kthen nje liste me claims te user
                return claims;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        //do te kthehe signingCredentials dhe nuk merr parameter
        private SigningCredentials CreateSigningCredentials()
        {
            //krijon SymmetricSecurityKey  me sekretin si nje vektor byte 
            return new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes("!SomethingSecret!12345abcd ergijnewr orwjngkjebwrkg reijbgkewbgrkhwberg")
                ),
                //specifikon cilen algoritem do te perdore per ta sajnuar kete token
                SecurityAlgorithms.HmacSha256
            );
        }
    }
}
