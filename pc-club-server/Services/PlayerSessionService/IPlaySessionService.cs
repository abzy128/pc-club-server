using pc_club_server.API.Models;

namespace pc_club_server.Services.PlayerSessionService
{
    public interface IPlaySessionService
    {
        Task<PlaySessionDto?> StartPlaySession(long userId, DateTime endTime);
        Task<bool> EndPlaySession(long userId);
        Task<PlaySessionDto?> GetPlaySession(long userId);
    }
}
