using Microsoft.EntityFrameworkCore;
using System;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace Backend_for_angular_CRUD.Model;
public class UsersContext: DbContext
{
	public DbSet<User> Users { get; set; }
}
