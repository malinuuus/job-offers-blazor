using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClassLib
{
	public class JobOffer : IModel
	{
		public int Id { get; set; }
		public string Position { get; set; }
		public int CompanyId { get; set; }
		[JsonIgnore]
		public Company Company { get; set; }
		public float LowerLimit { get; set; }
		public float UpperLimit { get; set; }
		public bool IsFullTime { get; set; }
		public int CityId { get; set; }
		[JsonIgnore]
		public City City { get; set; }
		public WorkMode WorkMode { get; set; }
		public string Description { get; set; }
		public DateTime ExpirationDate { get; set; }
		public DateTime CreatedAt { get; set; }
		[JsonIgnore]
		public List<Application> Applications { get; set; }

		[JsonIgnore]
		public TimeSpan TimeLeft => ExpirationDate - DateTime.Now;
		[JsonIgnore]
		public string IsFullTimeToString => IsFullTime ? "pełny etat" : "część etatu";
		[JsonIgnore]
		public string WorkModeToString => WorkModeDict[WorkMode];


		public static readonly Dictionary<WorkMode, string> WorkModeDict = new Dictionary<WorkMode, string>
		{
			{ WorkMode.Stationary, "praca stacjonarna" },
			{ WorkMode.Remote, "praca zdalna" },
			{ WorkMode.Hybrid, "praca hybrydowa" }
		};

		public JobOffer(string position, int companyId, float lowerLimit, float upperLimit, bool isFullTime, int cityId, WorkMode workMode, string description, DateTime expirationDate, DateTime createdAt)
		{
			Position = position;
			CompanyId = companyId;
			LowerLimit = lowerLimit;
			UpperLimit = upperLimit;
			IsFullTime = isFullTime;
			CityId = cityId;
			WorkMode = workMode;
			Description = description;
			ExpirationDate = expirationDate;
			CreatedAt = createdAt;
		}

		public IEnumerable<Application> GetApplications(IEnumerable<Application> applications)
		{
			return applications.Where(a => a.JobOfferId == Id);
		}

		public Company GetCompany(IEnumerable<Company> companies)
		{
			return companies.FirstOrDefault(c => c.Id == CompanyId);
		}

		public City GetCity(IEnumerable<City> cities)
		{
			return cities.FirstOrDefault(c => c.Id == CityId);
		}
	}

	public enum WorkMode
	{
		Remote,
		Stationary,
		Hybrid
	}
}
