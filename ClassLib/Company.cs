using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace ClassLib
{
	public class Company : IModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		[JsonIgnore]
		public List<Recruiter> Recruiters { get; set; }
		[JsonIgnore]
		public List<JobOffer> JobOffers { get; set; }

		public Company(string name)
		{
			Name = name;
		}

		public IEnumerable<Recruiter> GetRecruiters(IEnumerable<Recruiter> recruiters)
		{
			return recruiters.Where(r => r.CompanyId == Id);
		}

		public IEnumerable<JobOffer> GetJobOffers(IEnumerable<JobOffer> jobOffers)
		{
			return jobOffers.Where(j => j.CompanyId == Id);
		}
	}
}
