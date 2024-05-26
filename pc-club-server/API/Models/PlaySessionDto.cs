namespace pc_club_server.API.Models
{
    public class PlaySessionDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
