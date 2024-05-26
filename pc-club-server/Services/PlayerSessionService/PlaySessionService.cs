using AutoMapper;
using Microsoft.EntityFrameworkCore;
using pc_club_server.API.Models;
using pc_club_server.Core.Entities;
using pc_club_server.Infrastructure.Database;

namespace pc_club_server.Services.PlayerSessionService
{
    public class PlaySessionService(
        PcClubDbContext context,
        ILogger<PlaySessionService> logger,
        IMapper mapper) : IPlaySessionService
    {
        private readonly PcClubDbContext _context = context;
        private readonly ILogger<PlaySessionService> _logger = logger;
        private readonly IMapper _mapper = mapper;

        public Task<PlaySessionDto> EndPlaySession(long userId)
        {
            throw new NotImplementedException();
        }

        public async Task<PlaySessionDto?> GetPlaySession(long userId)
        {
            var dbPlaySession = await _context.PlaySessions
                .Where(x => x.UserId == userId
                    && x.StartTime < DateTime.UtcNow 
                    && x.EndTime > DateTime.UtcNow)
                .FirstOrDefaultAsync();
            return _mapper.Map<PlaySessionDto>(dbPlaySession);
        }

        public async Task<PlaySessionDto?> StartPlaySession(long userId, DateTime endTime)
        {
            endTime = endTime.ToUniversalTime();
            if(endTime < DateTime.UtcNow)
                return null;
            var existingPlaySession = await GetPlaySession(userId);
            if (existingPlaySession != null)
                return null;

            var dbPlaySession = new PlaySession
            {
                UserId = userId,
                StartTime = DateTime.UtcNow,
                EndTime = endTime
            };
            _context.PlaySessions.Add(dbPlaySession);
            await _context.SaveChangesAsync();
            return _mapper.Map<PlaySessionDto>(dbPlaySession);
        }
    }
}