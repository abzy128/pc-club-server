using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pc_club_server.Core.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required long Id { get; set; }
        [MaxLength(64)]
        public required string Username { get; set; }
        [MaxLength(256)]
        public required string Password { get; set; }
        [MaxLength(256)]
        public required string Email { get; set; }
        [MaxLength(15)]
        public required string PhoneNumber { get; set; }
        [MaxLength(32)]
        public required string SteamID { get; set; }
    }
}