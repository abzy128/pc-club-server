using AutoMapper;
using pc_club_server.API.Models;
using pc_club_server.Infrastructure.Database;

namespace pc_club_server.Services.UserService
{
    public sealed class UserService(
        PcClubDbContext context,
        ILogger<UserService> logger,
        IMapper mapper) : IUserService
    {
        private readonly PcClubDbContext _context = context ?? throw new ArgumentNullException(nameof(PcClubDbContext));
        private readonly ILogger<UserService> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public UserDto? GetUser(string? username)
        {
            ArgumentNullException.ThrowIfNull(username);
            var dbUser = _context.Users.Where(x => x.Username == username).FirstOrDefault();
            return _mapper.Map<UserDto>(dbUser);
        }

        public bool IsAuthenticated(string? password, string? passwordHash)
        {
            ArgumentNullException.ThrowIfNull(password);
            ArgumentNullException.ThrowIfNull(passwordHash);

            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        public UserDto? RegisterUser(UserInfo user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var dbUser = GetUser(user.Username);
            if (dbUser != null)
                return null;
            var newUser = new UserDto
            {
                Id = 0,
                Username = user.Username!,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return newUser;
        }
    }

}
