using pc_club_server.API.Models;

namespace pc_club_server.Services.JwtService
{
    public interface IJwtService
    {
        string GenerateToken(UserDto? userDto);
    }
}
