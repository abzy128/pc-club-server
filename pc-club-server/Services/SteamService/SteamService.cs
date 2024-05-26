using Microsoft.IdentityModel.Tokens;
using pc_club_server.API.Models;
using System.Xml.Serialization;

namespace pc_club_server.Services.SteamService
{
    public class SteamService : ISteamService
    {
        public string GetGameBannerURI(int gameID)
        {
            return $"https://steamcdn-a.akamaihd.net/steam/apps/{gameID}/library_600x900_2x.jpg";
        }

        public async Task<UserGameDataModel> GetUserGames(string steamID64)
        {
            var uri = $"https://steamcommunity.com/profiles/{steamID64}/games?tab=all&xml=1";
            var client = new HttpClient();
            var response = await client.GetAsync(uri);

            var xml = await response.Content.ReadAsStringAsync();
            
            var xmlSerializer = new XmlSerializer(typeof(UserGameDataModel));
            using var reader = new StringReader(xml);
            var games = (UserGameDataModel?)xmlSerializer.Deserialize(reader!) ?? throw new Exception($"Failed to deserialize list of apps for SteamID: {steamID64}");
            return games;
        }
    }
}
