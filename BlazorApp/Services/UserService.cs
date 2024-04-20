using ClassLib;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BlazorApp.Services
{
    public class UserService
    {
        private IDbContextFactory<ApplicationDbContext> _dbContextFactory;

        public UserService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Account?> GetAccountAsync(string email, string password)
        {
            var accounts = new List<Account>();

            await using (var db = _dbContextFactory.CreateDbContext())
            {
                accounts.AddRange(await db.Recruits.ToArrayAsync());
                accounts.AddRange(await db.Recruiters.Include(r => r.Company).ToArrayAsync());
            }

            var account = accounts.FirstOrDefault(a => a.Email == email);

            if (account != null && BCryptHash.VerifyPassword(password, account.Password))
                return account;

            return null;
        }

        public async Task<bool> TryCreateRecruitAsync(string firstName, string lastName, string email, string phoneNumber, string password)
        {
            var hashedPassword = BCryptHash.HashPassword(password);
            var recruit = new Recruit(firstName, lastName, email, hashedPassword, phoneNumber);
            bool success = true;

            using (var db = _dbContextFactory.CreateDbContext())
            {
                bool emailExists = await db.Recruits.AnyAsync(r => r.Email == email) || await db.Recruiters.AnyAsync(r => r.Email == email);

                if (emailExists)
                    success = false;
                else
                {
					db.Recruits.Add(recruit);
					await db.SaveChangesAsync();
				}
            }

            return success;
        }
    }
}
