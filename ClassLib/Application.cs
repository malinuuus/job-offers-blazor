using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace ClassLib
{
	public class Application : IModel
	{
		public int Id { get; set; }
		public int JobOfferId { get; set; }
		[JsonIgnore]
		public JobOffer JobOffer { get; set; }
		public int RecruitId { get; set; }
		[JsonIgnore]
		public Recruit Recruit { get; set; }
		public bool IsApproved { get; set; }
		public bool IsRejected { get; set; }
		public DateTime CreatedAt { get; set; }

		[JsonIgnore]
		public string Status => IsApproved ? "zaakceptowana" : IsRejected ? "odrzucona" : "dostarczona";

		public Application(int jobOfferId, int recruitId, bool isApproved, bool isRejected, DateTime createdAt)
		{
			JobOfferId = jobOfferId;
			RecruitId = recruitId;
			IsApproved = isApproved;
			IsRejected = isRejected;
			CreatedAt = createdAt;
		}

		public Recruit GetRecruit(IEnumerable<Recruit> recruits)
		{
			return recruits.FirstOrDefault(r => r.Id == RecruitId);
		}

		public JobOffer GetJobOffer(IEnumerable<JobOffer> jobOffers)
		{
			return jobOffers.FirstOrDefault(j => j.Id == JobOfferId);
		}
	}
}
