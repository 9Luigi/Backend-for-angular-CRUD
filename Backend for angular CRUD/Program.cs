
using Backend_for_angular_CRUD.EFContextSQLServer;
using Microsoft.EntityFrameworkCore;

namespace Backend_for_angular_CRUD;
public class Program
{
	public static void Main(string[] args)
	{
		List<User> usersToPost = new List<User>() {
			new User("Roman","Kudrik",26),
			new User("Vladimir","Sadkov",26),
			new User("Alexandra","Kravchenko",26),
			new User("Galina","Maizlina",21),
			new User("Dmitry","Surkov",29)
		};
		var usersContext = new UsersContext();
		var cRUDController = new CRUDController<User, UsersContext>(usersContext);

		var builder = WebApplication.CreateBuilder();
		builder.Services.AddCors();


		var app = builder.Build();
		app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
		app.MapGet("/AddList", async () =>
		{
			await cRUDController.ADD(usersToPost.ToArray());
		});
		app.MapGet("/", async (HttpResponse response) =>
		{
			response.ContentType = "application/json";
			var result = await cRUDController.SelectAsync();
			System.Diagnostics.Debug.WriteLine("All users send");
			return result;
		});
		app.MapGet("/api/users/{id}", async (HttpResponse response, string id) =>
		{
			User? userToFind;
			List<User> users = await cRUDController.SelectAsync(id);
			userToFind = users.FirstOrDefault();
			if(userToFind != null)
			{
				System.Diagnostics.Debug.WriteLine(userToFind!.Name + ": Send");
				return userToFind;
			}
			else
			{
				return null;
			}
		});
		app.MapDelete("/api/users/{id}", async (string id) =>
		{
			await cRUDController.Remove(id);
			System.Diagnostics.Debug.WriteLine("User with id:" + id + " Removed");
		});
		app.MapPost("api/users/", async (HttpRequest request) =>
		{
			User? sentUser = await request.ReadFromJsonAsync<User>();
			if (sentUser != null)
			{
				sentUser.Id = Guid.NewGuid().ToString();
				await cRUDController.ADD(sentUser);
			}
		});


		app.MapPut("/api/users/", async (HttpRequest request) =>
		{
			User? sendUser = await request.ReadFromJsonAsync<User>();
			User? foundUser = usersContext.Users.First(x => x.Id == sendUser!.Id);
			await cRUDController.AttachUpdate(sendUser!, foundUser!);
		});
		app.Run();
	}
}