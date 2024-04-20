using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace ClassLib
{
	public class City : IModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		[JsonIgnore]
		public List<JobOffer> JobOffers { get; set; }

		public City(string name)
		{
			Name = name;
		}

		public IEnumerable<JobOffer> GetJobOffers(IEnumerable<JobOffer> jobOffers)
		{
			return jobOffers.Where(j => j.CityId == Id);
		}
	}
}
