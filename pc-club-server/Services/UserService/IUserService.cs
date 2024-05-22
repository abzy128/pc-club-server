using pc_club_server.API.Models;

namespace pc_club_server.Services.UserService
{
    public interface IUserService
    {
        UserDto? GetUser(string? username);
        bool IsAuthenticated(string? password, string? passwordHash);
        UserDto? RegisterUser(UserInfo user);
    }
}
