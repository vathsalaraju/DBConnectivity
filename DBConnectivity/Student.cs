using System;
namespace DBConnectivity
{
    public class Student
    {
        
        private string _id;
        private string _name;
        private DateTime _date;

        public Student()
        {
        }

        public Student(string id, string name, string date)
        {
            Id = id;
            Name = name;
            Date = date;
        }
       public string Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Date { get => _date.ToString("yyyy-MM-dd"); 
        set => _date = DateTime.Parse(value); }

    }
}
