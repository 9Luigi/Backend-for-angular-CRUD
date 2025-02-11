using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace Backend_for_angular_CRUD.Services
{
	public class CRUDService<T, C> where T : class where C : DbContext
	{
		private C context;
		public CRUDService(C context)
		{
			this.context = context;
		}
		protected internal async Task ADD(params T[] entities)
		{
			switch (entities.Length)
			{
				case 1:
					context.Add(entities[0]); //if 1 argument send will be add 1 entity in db
					break;
				default:
					context.AddRange(entities);//if >1 argument send will be add all sended entities
					break;
			}
			try
			{
				var result = await context.SaveChangesAsync();
			}
			catch (Exception ex) { throw new Exception("Cannot save entity/es via CRUDController ADD method", ex); } 
		}
		protected internal async Task<List<T>> SelectAsync(params object[] ids)
		{
			var result = new List<T>();
			try
			{
				if (ids.Length == 0)
				{
					result = await context.Set<T>().ToListAsync();//if 0 arguments send return all dbset(all records in db)
				}
				foreach (var id in ids)
				{
					var entity = await context.Set<T>().FindAsync(id);
					if (entity != null)
					{
						result.Add(entity);
					}
				}
				return result; //if >=1 arguments send return entitys that contains send id/id's
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return new List<T>(); //cause don't return null
			}
		}
		protected internal async Task Remove(params string[] ids)
		{
			if (ids.Length == 0)
				throw new ArgumentException("No user IDs provided.");

			switch (ids.Length)
			{
				case 1: // Removing a single record
					if (Guid.TryParse(ids[0], out Guid guidId))
					{
						var entityToRemove = await context.Set<T>().FindAsync(guidId);
						if (entityToRemove != null)
						{
							context.Remove(entityToRemove);
							await context.SaveChangesAsync();
						}
					}
					break;

				default: // Removing multiple records
					var guidIds = ids.Select(id => Guid.TryParse(id, out var guid) ? guid : Guid.Empty)
									 .Where(guid => guid != Guid.Empty) // Exclude parsing errors
									 .ToList();

					var entitiesToRemove = await context.Set<T>()
						.Where(e => guidIds.Contains((Guid)e.GetType().GetProperty("Id")!.GetValue(e)!))
						.ToListAsync();

					if (entitiesToRemove.Count > 0)
					{
						context.RemoveRange(entitiesToRemove);
						await context.SaveChangesAsync();
					}
					break;
			}
		}

		protected internal async Task AttachUpdate(T sendEntity, T foundEntity) //changes edited fields in entity and push it into db
		{
			context.Attach(foundEntity);
			FieldsController.CopyFields(sendEntity, foundEntity); 
			await context.SaveChangesAsync();
		}
	}
}
