using Microsoft.IdentityModel.Tokens;
using Order_Service.src._01_Domain.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace Order_Service.src._03_Infrastructure.Services.Internal
{
    public class JwtTokenValidationService : IJwtTokenValidationService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenValidationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public Guid? GetUserIdFromToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                var claim = jwt.Claims.FirstOrDefault(c => c.Type == "sub") ?? jwt.Claims.FirstOrDefault(c => c.Type == "userId");

                if (claim != null && Guid.TryParse(claim.Value, out var userId))
                {
                    return userId;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
