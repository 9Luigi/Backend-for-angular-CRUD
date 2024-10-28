using Azure;
using Backend_for_angular_CRUD.EFContextSQLServer;
using Backend_for_angular_CRUD.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Backend_for_angular_CRUD
{
	public class CRUDController
	{
		protected internal UsersContext _userContext;
		public CRUDController(UsersContext userContext)
		{
			this._userContext = userContext;
		}
		protected internal void ADD(params User[] users)
		{
			switch (users.Length)
			{
				case 1:
					_userContext.Add(users[0]);
					break;
				default:
					_userContext.AddRange(users);
					break;
			}
		}
		//TODO to think send User object or id as already send
		//TODO <T> instead of downcasting
		protected internal object Select(params string[] idArray) 
		{
			switch (idArray.Length)
			{
				case 0:
					var users = this._userContext.Users.Select(u => new
					{
						u.Id,
						u.Name,
						u.Surname,
						u.Age
					}).ToList();
					return users;
				case 1:
					users = this._userContext.Users.Select(u => new
					{
						u.Id,
						u.Name,
						u.Surname,
						u.Age
					}).Where(u => u.Id == idArray[0]).ToList();
					return users;
				default:
					return null!; //TODO exception
			}
		}
	}
}
