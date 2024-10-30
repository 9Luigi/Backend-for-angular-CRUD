using Azure;
using Backend_for_angular_CRUD.EFContextSQLServer;
using Backend_for_angular_CRUD.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
namespace Backend_for_angular_CRUD
{
	public class CRUDController<T, C, S> where T : class where C : DbContext where S : DbSet<T>
	{
		protected internal C context;
		public CRUDController(C context)
		{
			this.context = context;
		}
		protected internal async Task ADD(params T[] entities)
		{
			switch (entities.Length)
			{
				case 1:
					context.Add(entities[0]);
					break;
				default:
					context.AddRange(entities);
					break;
			}
			try
			{
				var result = await context.SaveChangesAsync();
			}
			catch (Exception ex) { throw new Exception("Cannot save entity/es via CRUDController ADD method", ex); }
		}
		//TODO to think send User object or id as already send
		/*protected internal async Task<List<T>> Select(params string[] idArray)
		{
			switch (idArray.Length)
			{
				case 0:
					var users = this.context.Set<S>.Select(u => new
					{
						u.Id,
						u.Name,
						u.Surname,
						u.Age
					}).ToList();
					return users;
				case 1:
					var user = this.context.S.First(u => u.Id == idArray[0]);
					return user!;
				default:
					throw new NotImplementedException();
			}
		}*/
		/*protected internal void Remove(params string[] usersId)
		{
			User userToRemove;
			switch (usersId.Length)
			{
				case 0: throw new NotImplementedException();
				case 1:
					userToRemove = _userContext.Users.First(u => u.Id == usersId[0]);
					_userContext.Remove(userToRemove);
					_userContext.SaveChangesAsync();
					break;
				default:
					foreach (var id in usersId)
					{
						userToRemove = _userContext.Users.First(u => u.Id == id);
						_userContext.Remove(userToRemove);
						_userContext.SaveChangesAsync();
					}
					break;
			}
		}
		protected internal void AttachUpdate(User sendUser, User foundUser)
		{
			_userContext.Attach(foundUser);
			FieldsController.CopyFields<User>(sendUser, foundUser);
			_userContext.SaveChangesAsync();
		}*/
		#region Dependings
		private T CreateInstanceOF<T>(T entity) where T : new()
		{
			var instance = new T();
			var properties = typeof(T).GetProperties();

			foreach (var property in properties)
			{
				var value = typeof(S).GetProperty(property.Name)?.GetValue(entity);
				property.SetValue(instance, value);
			}

			return instance;
		}
		#endregion
	}
}
