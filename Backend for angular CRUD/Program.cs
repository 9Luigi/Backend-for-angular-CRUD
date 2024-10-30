
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
		var cRUDController = new CRUDController<User, UsersContext, DbSet<User>>(usersContext);

		var builder = WebApplication.CreateBuilder();
		builder.Services.AddCors();


		var app = builder.Build();
		app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
		app.MapGet("/AddList", () =>
		{
			cRUDController.ADD(usersToPost.ToArray());
		});
		app.MapGet("/", async (HttpResponse response) =>
		{
			response.ContentType = "application/json";
			response.Headers.Append("secret-id", "256");
			await response.WriteAsJsonAsync(cRUDController.Select());
			System.Diagnostics.Debug.WriteLine("Все пользователи  отправлены");
		});
		app.MapGet("/api/users/{id}", async (HttpResponse response, string id) =>
		{
			User? userToFind;
			if (cRUDController.Select(id) is User)
			{
				userToFind = cRUDController.Select(id) as User;
				await response.WriteAsJsonAsync(userToFind);
				System.Diagnostics.Debug.WriteLine(userToFind!.Name + ": Отправлен");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Select returns not User type");
				throw new Exception("Select returns not User type");
			}
		});
		app.MapPost("api/users/", async (HttpRequest request) =>
		{
			User? sentUser = await request.ReadFromJsonAsync<User>();
			sentUser!.Id = Guid.NewGuid().ToString();
			cRUDController.ADD(sentUser);
		});

		app.MapDelete("/api/users/{id}", async (string id) =>
		{
			cRUDController.Remove(id);
			System.Diagnostics.Debug.WriteLine("User with id:" + id + " Удален");
		});
		app.MapPut("/api/users/", async (HttpRequest request) =>
		{
			User? sendUser = await request.ReadFromJsonAsync<User>();
			User? foundUser = usersContext.Users.First(x => x.Id == sendUser!.Id);
			cRUDController.AttachUpdate(sendUser!, foundUser!);
		});
		app.Run();
	}
}