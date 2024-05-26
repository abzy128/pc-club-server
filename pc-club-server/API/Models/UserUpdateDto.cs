using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace pc_club_server.API.Models
{
    public class UserUpdateDto
    {
        [MaxLength(256)]
        public string? Email { get; set; }
        [MaxLength(15)]
        public string? PhoneNumber { get; set; }
        [MaxLength(32)]
        public string? SteamID { get; set; }
    }
}
