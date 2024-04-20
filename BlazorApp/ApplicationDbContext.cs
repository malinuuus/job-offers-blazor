using ClassLib;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Application> Applications { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<JobOffer> JobOffers { get; set; }
        public DbSet<Recruit> Recruits { get; set; }
        public DbSet<Recruiter> Recruiters { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobOffer>()
                .Ignore(j => j.TimeLeft)
                .Ignore(j => j.IsFullTimeToString)
                .Ignore(j => j.WorkModeToString);

            modelBuilder.Entity<Application>()
                .Ignore(a => a.Status);
        }
    }
}
