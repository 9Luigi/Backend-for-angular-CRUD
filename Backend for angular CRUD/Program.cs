using Backend_for_angular_CRUD.Model;
using System.Text.Json;

namespace Backend_for_angular_CRUD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<User> users = new List<User> { new User("1", "Roman", 24), new User("2", "Nazar", 24), new User("3", "Vladimir", 25) };
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors();

            var app = builder.Build();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod());

            app.MapGet("/", () => "HELLO");
            app.MapGet("/users/api", () => JsonSerializer.Serialize(users));
            app.MapPost("/users/api", async (HttpContext context) =>
            {
                User? user = await JsonSerializer.DeserializeAsync<User>(context.Request.Body);
                user!.Id =  Guid.NewGuid().ToString();
                Console.WriteLine(user!.Id);
                users.Add(user!);
            });
            app.MapPut("/users/api", async (HttpContext context) =>
            {
                User? user = await JsonSerializer.DeserializeAsync<User>(context.Request.Body);
                User? userToUpdate = users.FirstOrDefault(u => u.Id == user!.Id);
                users.Remove(userToUpdate!);
                userToUpdate = user;
                users.Add(userToUpdate!);
            });
            app.MapDelete("/users/api/{id?}", (string id) =>
            {
                User? userToDelete = users.FirstOrDefault(u => u.Id == id);
                users.Remove(userToDelete!);
            });

            app.Run();
        }
    }
}