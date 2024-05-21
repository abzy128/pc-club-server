using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using pc_club_server.API.Models;
using pc_club_server.Core.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace pc_club_server.Services.JwtService
{
    public sealed class JwtService(IOptions<JwtOptions> options) : IJwtService
    {
        private readonly JwtOptions _options = options.Value ?? throw new ArgumentNullException(nameof(JwtOptions));

        public string GenerateToken(UserDto? userDto)
        {
            ArgumentNullException.ThrowIfNull(userDto);

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Key));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Name, userDto.Username),
                    new(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
                    new(ClaimTypes.Email, userDto.Email),
                    new(ClaimTypes.MobilePhone, userDto.PhoneNumber)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
