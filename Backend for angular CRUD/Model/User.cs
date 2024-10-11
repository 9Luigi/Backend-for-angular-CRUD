using System.Diagnostics.Metrics;

namespace Backend_for_angular_CRUD.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public User(string name, string Surname)
        {
			this.Id = Guid.NewGuid();
			this.Name = name;
            this.Surname = Surname;
            this.Age = 1;
		}
    }
}
