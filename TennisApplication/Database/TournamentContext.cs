using Microsoft.EntityFrameworkCore;
using TennisApplication.Models;

namespace TennisApplication.Database
{
    public class TournamentContext : DbContext
    {
        public TournamentContext(DbContextOptions<TournamentContext> options) : base (options)
        {
            
        }

        public DbSet<Tournament> Tournaments { get; set; }
    }
}