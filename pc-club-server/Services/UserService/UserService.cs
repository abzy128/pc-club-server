using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public async Task<UserDto?> GetUser(long id)
        {
            if(id <= 0)
                return null;
            var dbUser = await _context.Users.FindAsync(id);
            return _mapper.Map<UserDto>(dbUser);
        }
        public async Task<UserDto?> GetUser(string? username)
        {
            ArgumentNullException.ThrowIfNull(username);
            var dbUser = await _context.Users.Where(x => x.Username == username).FirstOrDefaultAsync();
            return _mapper.Map<UserDto>(dbUser);
        }

        public bool IsAuthenticated(string? password, string? passwordHash)
        {
            ArgumentNullException.ThrowIfNull(password);
            ArgumentNullException.ThrowIfNull(passwordHash);

            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
        public async Task<UserDto?> RegisterUser(UserInfo user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var dbUser = await GetUser(user.Username);
            if (dbUser != null)
                return null;
            var newUser = new UserDto
            {
                Id = 0,
                Username = user.Username!,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<UserDto?> UpdateUser(int id, UserUpdateDto user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var dbUser = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            if (dbUser == null)
                return null;

            dbUser.Email = user.Email;
            dbUser.PhoneNumber = user.PhoneNumber;
            dbUser.SteamID = user.SteamID;

            await _context.SaveChangesAsync();
            return _mapper.Map<UserDto>(dbUser);
        }

        public async Task<bool> UpdatePassword(int userId, string password)
        {
            ArgumentNullException.ThrowIfNull(password);

            var dbUser = _context.Users.Where(x => x.Id == userId).FirstOrDefault();
            if (dbUser == null)
                return false;

            dbUser.Password = BCrypt.Net.BCrypt.HashPassword(password);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
