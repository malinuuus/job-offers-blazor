using ClassLib;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Services
{
	public class ApplicationsService
	{
		IDbContextFactory<ApplicationDbContext> _dbContextFactory;

		public ApplicationsService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}

		public async Task<IEnumerable<Application>> GetRecruitsApplicationsAsync(int recruitId)
		{
			using (var db = _dbContextFactory.CreateDbContext())
			{
				return await db.Applications
					.Where(a => a.RecruitId == recruitId)
					.Include(a => a.JobOffer).ThenInclude(j => j.Company)
					.Include(a => a.JobOffer).ThenInclude(j => j.City)
					.OrderByDescending(j => j.CreatedAt)
					.ToListAsync();
			}
		}
		
		public async Task<bool> ApproveApplicationAsync(Application application)
		{
			using (var db = _dbContextFactory.CreateDbContext())
			{
				application.IsApproved = true;
				db.Entry(application).Property(a => a.IsApproved).IsModified = true;
				int rows = await db.SaveChangesAsync();
				return rows == 1;
			}
		}

		public async Task<bool> RejectApplicationAsync(Application application)
		{
			using (var db = _dbContextFactory.CreateDbContext())
			{
				application.IsRejected = true;
				db.Entry(application).Property(a => a.IsRejected).IsModified = true;
				int rows = await db.SaveChangesAsync();
				return rows == 1;
			}
		}
	}
}
