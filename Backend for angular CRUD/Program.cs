using Backend_for_angular_CRUD.Model;
using System.Net;
using System.Net.Http.Json;

namespace Backend_for_angular_CRUD
{
	public class Program
	{
		public static void Main(string[] args)
		{
			List<User> users = new List<User>() { new User("Vladimir", DateTime.Now), new User("Nikita", DateTime.MinValue), new User("Egor", DateTime.MaxValue) };
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
			app.MapGet("/api/users/{id}", async (HttpResponse response, int id) =>
			{
				User? userToFind = users.FirstOrDefault(u => u.Id == id);
				await response.WriteAsJsonAsync(userToFind);
				System.Diagnostics.Debug.WriteLine(userToFind?.Name + " Отправлен");
			});
			app.MapDelete("/api/users/{id}", async (int id) =>
			{
				User? useToDelete = users.FirstOrDefault(u => u.Id == id);
				users.Remove(useToDelete!);
				System.Diagnostics.Debug.WriteLine(useToDelete?.Name + " Удален");
			});
			app.MapPut("/api/users/", async (HttpRequest request) =>
			{
				User? userToPut = await request.ReadFromJsonAsync<User>();
				User? editedUser = users.FirstOrDefault(u => u.Id == userToPut?.Id);
				users.Remove(userToPut!);
				users.Add(editedUser!);
				/*foreach(User user in users)
				{
					System.Diagnostics.Debug.WriteLine(userToPut);
				}*/
				System.Diagnostics.Debug.WriteLine(userToPut.ToString());
				//System.Diagnostics.Debug.WriteLine(editedUser.ToString());

			});
			app.Run();
		}
	}
}