namespace BlazorApp.Authentication
{
	public class UserSession
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Role { get; set; }
		public int? CompanyId { get; set; }
		public string? CompanyName { get; set; }
	}
}
