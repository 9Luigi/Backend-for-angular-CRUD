using Backend_for_angular_CRUD.Model;
using System.Net;
using System.Net.Http.Json;

namespace Backend_for_angular_CRUD
{
	public class Program
	{
		public static void Main(string[] args)
		{
			List<User> users = new List<User>() { new User("Vladimir", "Sadkov"), new User("Nikita", "Kovalev"), new User("Egor", "Sosed") };
			var builder = WebApplication.CreateBuilder();
			builder.Services.AddCors();


			var app = builder.Build();
			app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

			app.MapGet("/", async (HttpResponse response) =>
			{
				response.ContentType = "application/json";
				response.Headers.Append("secret-id", "256");
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
				User? editedUser = new User("","") {
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
}