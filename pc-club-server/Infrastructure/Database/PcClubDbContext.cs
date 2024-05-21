using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using pc_club_server.Core.Entities;
using pc_club_server.Core.Options;

namespace pc_club_server.Infrastructure.Database
{
    public class PcClubDbContext(DbContextOptions<PcClubDbContext> options, IOptions<ConnectionStringOptions> connectionStrings) : DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        public DbSet<User> Users { get; set; }
    }
}
