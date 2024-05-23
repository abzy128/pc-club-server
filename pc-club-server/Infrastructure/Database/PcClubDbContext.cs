using Microsoft.EntityFrameworkCore;
using pc_club_server.Core.Entities;

namespace pc_club_server.Infrastructure.Database
{
    public class PcClubDbContext(DbContextOptions<PcClubDbContext> options) : DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            Database.EnsureCreated();
            Database.Migrate();
        }
        public DbSet<User> Users { get; set; }
    }
}
