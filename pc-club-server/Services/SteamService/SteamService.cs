using pc_club_server.API.Models;
using System.Xml;

namespace pc_club_server.Services.SteamService
{
    public class SteamService : ISteamService
    {
        
        public string GetGameBannerURI(int gameID)
        {
            return $"https://steamcdn-a.akamaihd.net/steam/apps/{gameID}/library_600x900_2x.jpg";
        }

        public async Task<UserGameDataModel> GetUserGames(string steamID)
        {
            var uri = $"https://steamcommunity.com/profiles/{steamID}/games?tab=all&xml=1";
            var client = new HttpClient();
            var response = await client.GetAsync(uri);

            var xmlString = await response.Content.ReadAsStringAsync();
            var xml = new XmlDocument();
            xml.LoadXml(xmlString);

            var games = xml.SelectSingleNode("//gamesList/games");
            if(games == null)
                return new UserGameDataModel { SteamID = steamID, Games = [] };
            var gameDataList = new List<GameDataModel>();
            foreach(XmlNode game in games)
            {
                var gameData = new GameDataModel
                {
                    AppID = int.Parse(game.SelectSingleNode("appID")?.InnerText ?? "0"),
                    Name = game.SelectSingleNode("name")?.InnerText ?? string.Empty,
                    Logo = game.SelectSingleNode("logo")?.InnerText ?? string.Empty,
                    StoreLink = game.SelectSingleNode("storeLink")?.InnerText ?? string.Empty,
                    HoursLast2Weeks = double.Parse(game.SelectSingleNode("hoursLast2Weeks")?.InnerText ?? "0"),
                    HoursOnRecord = double.Parse(game.SelectSingleNode("hoursOnRecord")?.InnerText ?? "0"),
                    StatsLink = game.SelectSingleNode("statsLink")?.InnerText ?? string.Empty,
                    GlobalStatsLink = game.SelectSingleNode("globalStatsLink")?.InnerText ?? string.Empty
                };
                gameDataList.Add(gameData);
            }
            var gameDataModel = new UserGameDataModel
            {
                SteamID = steamID,
                Games = gameDataList
            };
            return gameDataModel;
        }
    }
}
