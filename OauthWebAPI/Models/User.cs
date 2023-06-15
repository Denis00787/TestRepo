
using System;
namespace OauthWebAPI.Models
{
	public class User
	{
		public int ID { get; set; }
		public string Username { get; set; }
		public string Pass { get; set; }
		public DateTime LastAccess { get; set; }
	}
}

