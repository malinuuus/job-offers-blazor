using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace ClassLib
{
	public class Recruit : Account
	{
		public string PhoneNumber { get; set; }
		[JsonIgnore]
		public List<Application> Applications { get; set; }

		public Recruit(string firstName, string lastName, string email, string password, string phoneNumber) : base(firstName, lastName, email, password)
		{
			PhoneNumber = phoneNumber;
		}

		public IEnumerable<Application> GetApplications(IEnumerable<Application> applications)
		{
			return applications.Where(a => a.RecruitId == Id);
		}
	}
}
