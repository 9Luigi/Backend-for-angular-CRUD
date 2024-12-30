using Backend_for_angular_CRUD.EFContextSQLServer;
using Backend_for_angular_CRUD.Services;
using Microsoft.EntityFrameworkCore;

namespace Backend_for_angular_CRUD
{
	public class Program
	{
		public static void Main(string[] args)
		{
			List<User> usersToPost = new List<User>
			{
				new User("Roman", "Kudrik", 26),
				new User("Vladimir", "Sadkov", 26),
				new User("Alexandra", "Kravchenko", 26),
				new User("Galina", "Maizlina", 21),
				new User("Dmitry", "Surkov", 29)
			};

			var builder = WebApplication.CreateBuilder();
			builder.Services.AddCors();
			builder.Services.AddScoped<CRUDService<User, UsersContext>>();
			builder.Services.AddScoped<UsersContext>();
			builder.Services.AddControllers(); // Add controllers

			var app = builder.Build();
			app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
			app.MapGet("/", context =>
			{
				context.Response.Redirect("/api/users");
				return Task.CompletedTask;
			});

			// Hand init data
			app.MapGet("/AddList", async (CRUDService<User, UsersContext> cRUDController) =>
			{
				await cRUDController.ADD(usersToPost.ToArray());
			});

			app.MapControllers(); // Connecting all controller routes
			app.Run();
		}
	}
}
