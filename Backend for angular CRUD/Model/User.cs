using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace Backend_for_angular_CRUD
{
	public class User
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public int Age { get; set; }
		public byte[]? RowVersion { get; set; } //value set via Entity Framework, never null
		public User(string name, string Surname, int age)
		{
			this.Id = Guid.NewGuid();
			this.Name = name;
			this.Surname = Surname;
			this.Age = age;
		}

	}
}
