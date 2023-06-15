using System;
using OauthWebAPI.Models;

namespace OauthWebAPI.DB
{
	public interface IDbRepository
	{
         Task InsertAsync(User user);
         Task UpdateAsync(User user);
         Task DeleteAsync(int id);

    }
}

