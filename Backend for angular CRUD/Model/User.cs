using System.Diagnostics.Metrics;

namespace Backend_for_angular_CRUD.Model
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public int Age { get; set; }
        public DateTime BirthDay { get; set; }
        public static int counter;
        public User(string name, DateTime birthDay)
        {
            counter++;
			this.Id = counter;
			this.Name = name;
			this.BirthDay = birthDay;
			var today = DateTime.Today;
			this.Age = today.Year - BirthDay.Year;
			if (BirthDay.Date > today.AddYears(-this.Age)) this.Age--;
			System.Diagnostics.Debug.WriteLine(this.Age);
			System.Diagnostics.Debug.WriteLine(this.BirthDay);
            SurName = "da";
		}
    }
}
