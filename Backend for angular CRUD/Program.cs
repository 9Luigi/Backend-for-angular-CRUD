
using Backend_for_angular_CRUD.EFContextSQLServer;
using Microsoft.EntityFrameworkCore;

namespace Backend_for_angular_CRUD;
public class Program
{
	public static void Main(string[] args)
	{
		List<User> usersToPost = new List<User>() { //set to init db
			new User("Roman","Kudrik",26),
			new User("Vladimir","Sadkov",26),
			new User("Alexandra","Kravchenko",26),
			new User("Galina","Maizlina",21),
			new User("Dmitry","Surkov",29)
		};

		var builder = WebApplication.CreateBuilder();
		builder.Services.AddCors();
		builder.Services.AddScoped<CRUDController<User, UsersContext>>(); //for dependency injection
		builder.Services.AddScoped<UsersContext>();

		var app = builder.Build();
		app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); //cross domen for different dns front/backend
		
		app.MapGet("/AddList", async (CRUDController<User, UsersContext> cRUDController) =>
		{
			await cRUDController.ADD(usersToPost.ToArray()); //initiate db via route
		});
		app.MapGet("/", async (CRUDController<User,UsersContext> cRUDController, HttpResponse response) =>
		{
			response.ContentType = "application/json";
			var result = await cRUDController.SelectAsync();
			System.Diagnostics.Debug.WriteLine("All users send");
			return result; //return full dbSet
		});
		app.MapGet("/api/users/{id}", async (CRUDController<User, UsersContext> cRUDController, string id) =>
		{
			User? userToFind;
			List<User> users = await cRUDController.SelectAsync( id);
			userToFind = users.FirstOrDefault();
			if(userToFind != null)
			{
				System.Diagnostics.Debug.WriteLine(userToFind!.Name + ": Send");
				return userToFind; //return user from dbSet
			}
			else
			{
				return null; //handled in CRUDController
			}
		});
		app.MapDelete("/api/users/{id}", async (CRUDController<User, UsersContext> cRUDController, string id) =>
		{
			await cRUDController.Remove(id); //remove user record from table in database
			System.Diagnostics.Debug.WriteLine("User with id:" + id + " Removed");
		});
		app.MapPost("api/users/", async (CRUDController<User, UsersContext> cRUDController, HttpRequest request) =>
		{
			User? sentUser = await request.ReadFromJsonAsync<User>();
			if (sentUser != null)
			{
				sentUser.Id = Guid.NewGuid(); //generate new user's uniq id
				await cRUDController.ADD(sentUser); //Create and push user's entity to database
			}
		});


		app.MapPut("/api/users/", async (CRUDController<User, UsersContext> cRUDController, HttpRequest request, UsersContext userContext) =>
		{
			User? sendUser = await request.ReadFromJsonAsync<User>(); //attempt to read user's entity from resieved json
			if(sendUser != null)
			{
				User? foundUser = userContext.Users.First(x => x.Id == sendUser!.Id); //if attempt above succed search that entity in database
				await cRUDController.AttachUpdate(sendUser!, foundUser!); //change all edited(don't matched) fields via refliction in CRUDController
			}
		});
		app.Run();
	}
}