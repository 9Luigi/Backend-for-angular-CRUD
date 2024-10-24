﻿using System.Diagnostics.Metrics;

namespace Backend_for_angular_CRUD
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public User(string name, string Surname, int age)
        {
			this.Id = Guid.NewGuid().ToString();
			this.Name = name;
            this.Surname = Surname;
            this.Age = age;
		}
    }
}
