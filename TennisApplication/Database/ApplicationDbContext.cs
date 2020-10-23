using Microsoft.EntityFrameworkCore;
using TennisApplication.Models;

namespace TennisApplication.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Enrolment>().HasKey(table => new
            {
                table.TournamentId, table.UserId
            });
        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Enrolment> Enrolments { get; set; }
    }
}