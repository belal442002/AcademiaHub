using AcademiaHub.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AcademiaHub.Services
{
    public class TokenService : ITokenService
    {
        private readonly AcademiaHubDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public TokenService(AcademiaHubDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public string GenerateJWT(IdentityUser user, List<string> roles)
        {
            List<Claim> claims = new List<Claim>()
            { 
                new Claim("userName", user.UserName!),
                new Claim("userId", user.Id)
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            var expirationDate = DateTime.UtcNow.AddMinutes(45);
            claims.Add(new Claim("exp", new DateTimeOffset(expirationDate).ToUnixTimeSeconds().ToString()));

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                //issuer: issuer,
                //audience: audience,
                claims: claims,
                //expires: DateTime.UtcNow.AddMinutes(45),
                //signingCredentials: credentials
                expires: expirationDate,
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
