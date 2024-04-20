using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace ClassLib
{
	public class Recruiter : Account
	{
		public int CompanyId { get; set; }
		[JsonIgnore]
		public Company Company { get; set; }

		public Recruiter(string firstName, string lastName, string email, string password, int companyId) : base(firstName, lastName, email, password)
		{
			CompanyId = companyId;
		}

		public Company GetCompany(IEnumerable<Company> companies)
		{
			return companies.FirstOrDefault(c => c.Id == CompanyId);
		}
	}
}
