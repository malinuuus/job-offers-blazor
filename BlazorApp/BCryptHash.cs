using System.Diagnostics;

namespace BlazorApp
{
	public class BCryptHash
	{
		public static string HashPassword(string password)
		{
			var salt = BCrypt.Net.BCrypt.GenerateSalt();
			var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
			return hashedPassword;
		}

		public static bool VerifyPassword(string password, string hashedPassword)
		{
			try
			{
				bool isCorrect = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
				return isCorrect;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
				return false;
			}
		}
	}
}
