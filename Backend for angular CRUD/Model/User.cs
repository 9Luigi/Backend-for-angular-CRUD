using System.Diagnostics.Metrics;

namespace Backend_for_angular_CRUD.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public static int counter;
        public User(string name, int age)
        {
            counter++;
            Id = counter;
            Name = name;
            Age = age;
        }
    }
}
