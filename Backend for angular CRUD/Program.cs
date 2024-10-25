
using Backend_for_angular_CRUD.EFContextSQLServer;

namespace Backend_for_angular_CRUD;
public class Program
{
	public static void Main(string[] args)
	{
		List<User> users = new List<User>() {
			new User("Roman","Kudrik",26),
			new User("Vladimir","Sadkov",26), 
			new User("Alexandra","Kravchenko",26), 
			new User("Galina","Maizlina",21), 
			new User("Dmitry","Surkov",29)
		};

		var builder = WebApplication.CreateBuilder();
		builder.Services.AddCors();


		var app = builder.Build();
		app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
		app.MapGet("/AddList", () =>
		{
			var context = new UsersContext();
			context.AddRange(users);
			context.SaveChanges();
		});
		app.MapGet("/", async (HttpResponse response) =>
		{
			var context = new UsersContext();
			response.ContentType = "application/json";
			response.Headers.Append("secret-id", "256");

			var users = context.Users.Select(u => new
			{
				u.Id,
				u.Name,
				u.Surname,
				u.Age
			}).ToList();

			await response.WriteAsJsonAsync(users);
			System.Diagnostics.Debug.WriteLine("Все пользователи  отправлены");
		});
		app.MapGet("/api/users/{id}", async (HttpResponse response, string id) =>
		{
			User? userToFind = users.FirstOrDefault(u => u.Id.ToString() == id);
			await response.WriteAsJsonAsync(userToFind);
			System.Diagnostics.Debug.WriteLine(userToFind?.Name + " Отправлен");
		});
		app.MapDelete("/api/users/{id}", async (string id) =>
		{
			User? useToDelete = users.FirstOrDefault(u => u.Id.ToString() == id);
			users.Remove(useToDelete!);
			System.Diagnostics.Debug.WriteLine(useToDelete?.Name + " Удален");
		});
		app.MapPut("/api/users/", async (HttpRequest request) =>
		{
			User? requestedUser = await request.ReadFromJsonAsync<User>();
			User? userToDelete = users.FirstOrDefault(u => u.Id == requestedUser!.Id); //TODO why can't just remove requestedUser?
			User? editedUser = new User("", "", 0)
			{
				Name = requestedUser!.Name,
				Surname = requestedUser.Surname,
				Age = requestedUser.Age,
				Id = requestedUser.Id
			};

			var index = users.IndexOf(userToDelete!);
			users.RemoveAt(index);
			users.Insert(index, editedUser);
		});
		app.MapPost("api/users/", async (HttpRequest request) =>
		{
			User? sentUser = await request.ReadFromJsonAsync<User>();
			sentUser!.Id = Guid.NewGuid().ToString();
			users.Add(sentUser!);
		});
		app.Run();
	}
}