using Microsoft.EntityFrameworkCore;
namespace Backend_for_angular_CRUD
{
	public class CRUDController<T, C> where T : class, IEntity where C : DbContext
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
		protected internal async Task<List<T>> SelectAsync(params object[] ids)
		{
			var result = new List<T>();
			try
			{
				if (ids.Length == 0)
				{
					result = await context.Set<T>().ToListAsync();
				}
				foreach (var id in ids)
				{
					var entity = await context.Set<T>().FindAsync(id);
					if (entity != null)
					{
						result.Add(entity);
					}
				}
				return result;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new List<T>();
			}
		}

		protected internal async Task Remove(params string[] ids)
		{
			T entityToRemove;
			switch (ids.Length)
			{
				case 0: throw new NotImplementedException("No user IDs provided.");
				case 1:
					entityToRemove = await context.Set<T>().FirstOrDefaultAsync(u => u.Id == ids[0]);
					if (entityToRemove != null)
					{
						context.Remove(entityToRemove);
						await context.SaveChangesAsync();
					}
					break;
				default:
					var entitiesToRemove = new List<T>();
					foreach (var id in ids)
					{
						entityToRemove = context.Set<T>().Where(u => u.Id == id);
						entitiesToRemove.Add(entityToRemove);
					}
					if (entitiesToRemove.Count > 0)
					{
						context.RemoveRange(entitiesToRemove);
						await context.SaveChangesAsync();
					}
					break;
			}
		}
		/*protected internal void AttachUpdate(User sendUser, User foundUser)
		{
			_userContext.Attach(foundUser);
			FieldsController.CopyFields<User>(sendUser, foundUser);
			_userContext.SaveChangesAsync();
		}*/
	}
}
