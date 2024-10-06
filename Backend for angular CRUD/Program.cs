using Backend_for_angular_CRUD.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Backend_for_angular_CRUD
{
	public class Program
	{
		public static void Main(string[] args)
		{
			List<User> users = new List<User>() { new User("Vladimir", 24), new User("Nikita", 25), new User("Egor", 26) };
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
				System.Diagnostics.Debug.WriteLine(userToFind?.Name+" Отправлен");
			});
			app.MapDelete("/api/users/{id}", async (HttpResponse response, int id) =>
			{
				User? useToDelete = users.FirstOrDefault(u=> u.Id ==id);
				users.Remove(useToDelete!);
				System.Diagnostics.Debug.WriteLine(useToDelete?.Name+" Удален");
			});
			app.Run();
		}
	}
}