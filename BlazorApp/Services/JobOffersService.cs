using ClassLib;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Services
{
    public class JobOffersService
    {
        private IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public JobOffersService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<IEnumerable<JobOffer>> GetJobOffersAsync(string query = "")
        {
            using (var db = _dbContextFactory.CreateDbContext())
            {
                return await db.JobOffers
                    .Where(j => j.ExpirationDate > DateTime.Now)
                    .Include(j => j.Company)
                    .Include(j => j.City)
                    .Where(j => j.Position.Contains(query) || j.Company.Name.Contains(query) || j.Description.Contains(query))
                    .OrderByDescending(j => j.CreatedAt)
                    .ToListAsync();
            }
        }

		public async Task<IEnumerable<JobOffer>> GetJobOffersAsync(int companyId, string query = "")
        {
            var jobOffers = await GetJobOffersAsync(query);
            return jobOffers.Where(j => j.CompanyId == companyId);
        }

        public async Task<JobOffer?> GetJobOfferByIdAsync(int id)
        {
            using (var db = _dbContextFactory.CreateDbContext())
            {
                return await db.JobOffers
                    .Where(j => j.ExpirationDate > DateTime.Now)
                    .Include(j => j.Company)
                    .Include(j => j.City)
                    .Include(j => j.Applications.OrderByDescending(a => a.CreatedAt)).ThenInclude(a => a.Recruit)
                    .FirstOrDefaultAsync(j => j.Id == id);
            }
        }

        public async Task<bool> ApplyForJobAsync(int jobOfferId, int recruitId)
        {
            var app = new Application(jobOfferId, recruitId, false, false, DateTime.Now);

            using (var db = _dbContextFactory.CreateDbContext())
            {
                bool appliedBefore = db.Applications.Any(a => a.JobOfferId == jobOfferId && a.RecruitId == recruitId);

                if (appliedBefore)
                    return false;

                db.Applications.Add(app);
                await db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            using (var db = _dbContextFactory.CreateDbContext())
            {
                return await db.Cities.OrderBy(c => c.Name).ToListAsync();
            }
        }

        public async Task<JobOffer?> AddJobOfferAsync(JobOffer jobOffer)
        {
            if (string.IsNullOrEmpty(jobOffer.Position))
                return null;
            
            await using var db = _dbContextFactory.CreateDbContext();
            var newJobOffer = db.JobOffers.Add(jobOffer);
            await db.SaveChangesAsync();
            return newJobOffer.Entity;
        }
	}
}
