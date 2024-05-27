using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace pc_club_server.API.Models
{
    public class UserDto
    {
        public required long Id { get; set; }
        [MaxLength(64)]
        public required string Username { get; set; }
        [MaxLength(256)]
        [JsonIgnore]
        public string Password { get; set; } = string.Empty;
        [MaxLength(256)]
        public string? Email { get; set; }
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }
        [MaxLength(32)]
        public string? SteamID { get; set; }
    }
}
