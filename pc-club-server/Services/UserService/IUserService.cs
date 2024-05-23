using pc_club_server.API.Models;

namespace pc_club_server.Services.UserService
{
    public interface IUserService
    {
        Task<UserDto?> GetUser(string? username);
        bool IsAuthenticated(string? password, string? passwordHash);
        Task<UserDto?> RegisterUser(UserInfo user);
        Task<UserDto?> UpdateUser(UserDto user);
        Task<bool> UpdatePassword(int userId, string password);
    }
}
