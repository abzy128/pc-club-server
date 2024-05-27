using pc_club_server.API.Models;

namespace pc_club_server.Services.SteamService
{
    public interface ISteamService
    {
        string GetGameBannerURI(long gameID);
        Task<UserGameDataModel> GetUserGames(string steamID64);
    }
}
