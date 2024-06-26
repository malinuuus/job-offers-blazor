﻿using System;

namespace ClassLib
{
	public abstract class Account : IModel
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }

		public Account(string firstName, string lastName, string email, string password)
		{
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Password = password;
		}
	}
}
