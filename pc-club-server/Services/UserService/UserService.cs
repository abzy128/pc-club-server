using pc_club_server.API.Models;
using pc_club_server.Infrastructure.Database;

namespace pc_club_server.Services.UserService
{
    public sealed class UserService(
        PcClubDbContext context,
        ILogger<UserService> logger) : IUserService
    {
        private readonly PcClubDbContext _context = context ?? throw new ArgumentNullException(nameof(PcClubDbContext));
        private readonly ILogger<UserService> _logger = logger;

        public UserDto? GetUser(string? username)
        {
            ArgumentNullException.ThrowIfNull(username);

            return (UserDto?)_context.Users.Where(x => x.Username == username).FirstOrDefault();
        }

        public bool IsAuthenticated(string? password, string? passwordHash)
        {
            ArgumentNullException.ThrowIfNull(password);
            ArgumentNullException.ThrowIfNull(passwordHash);

            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }

}
