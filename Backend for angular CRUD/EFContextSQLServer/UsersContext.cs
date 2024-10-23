using Microsoft.EntityFrameworkCore;
/// <summary>
/// Summary description for Class1
/// </summary>
namespace Backend_for_angular_CRUD.EFContextSQLServer;
public class UsersContext : DbContext
{
	internal DbSet<User> Users { get; set; }
	string ConnectionString = @"Server=" +
		Environment.MachineName +
		";DataBase=Users;User Id=Adm_RKudrik;Password=g7DcA)RH^qZw;MultipleActiveResultSets=true;Encrypt=False";
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer(ConnectionString);
		optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfiguration(new UserConfig());
	}
}
