using Backend_for_angular_CRUD.Model;
using System.Net;
using System.Net.Http.Json;

namespace Backend_for_angular_CRUD
{
	public class Program
	{
		public static void Main(string[] args)
		{
			List<User> users = new List<User>() { new User("Vladimir", "Vladimir"), new User("Nikita", "DateTime.MinValue"), new User("Egor", "DateTime.MaxValue") };
			var builder = WebApplication.CreateBuilder();
			builder.Services.AddCors();


			var app = builder.Build();
			app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

			app.MapGet("/", async (HttpResponse response) =>
			{
				response.ContentType = "application/json";
				response.Headers.Append("secret-id", "256");
				await response.WriteAsJsonAsync(users);
				System.Diagnostics.Debug.WriteLine("��� ������������  ����������");
			});
			app.MapGet("/api/users/{id}", async (HttpResponse response, string id) =>
			{
				User? userToFind = users.FirstOrDefault(u => u.Id.ToString() == id);
				await response.WriteAsJsonAsync(userToFind);
				System.Diagnostics.Debug.WriteLine(userToFind?.Name + " ���������");
			});
			app.MapDelete("/api/users/{id}", async (string id) =>
			{
				User? useToDelete = users.FirstOrDefault(u => u.Id.ToString() == id);
				users.Remove(useToDelete!);
				System.Diagnostics.Debug.WriteLine(useToDelete?.Name + " ������");
			});
			app.MapPut("/api/users/", async (HttpRequest request) =>
			{
				User? userToPut = await request.ReadFromJsonAsync<User>();
				User? editedUser = new User("","") {
					Name = userToPut.Name,
					Surname = userToPut.Surname
				};
				editedUser.Id = userToPut.Id;
				users.Remove(userToPut);
				users.Add(editedUser);
				/*foreach(User user in users)
				{
					System.Diagnostics.Debug.WriteLine(userToPut);
				}*/
				System.Diagnostics.Debug.WriteLine(userToPut.Id);
				System.Diagnostics.Debug.WriteLine(userToPut.Name);
				System.Diagnostics.Debug.WriteLine(userToPut.Surname);
				System.Diagnostics.Debug.WriteLine(editedUser.Id);
				System.Diagnostics.Debug.WriteLine(editedUser.Name);
				System.Diagnostics.Debug.WriteLine(editedUser.Surname);
				//System.Diagnostics.Debug.WriteLine(editedUser.ToString());

			});
			app.Run();
		}
	}
}