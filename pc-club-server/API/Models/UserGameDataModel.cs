namespace pc_club_server.API.Models
{
    public class UserGameDataModel
    {
        public int SteamID64 { get; set; }
        public List<GameDataModel> Games { get; set; } = [];
    }
    public class GameDataModel
    {
        public int AppID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string StoreLink { get; set; } = string.Empty;
        public double HoursLast2Weeks { get; set; }
        public double HoursOnRecord { get; set; }
        public string StatsLink { get; set; } = string.Empty;
        public string GlobalStatsLink { get; set; } = string.Empty;
    }
}
