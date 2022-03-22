using System.Configuration;
using System.Data.SqlClient;

namespace DBConnectivity
{
    public class Enroll
    {
        private Student _student;
        private Course _course;
        private DateTime _enrollmentDate;       
        public Enroll()
        {
        }

        public Enroll(Student student, Course course, DateTime enrollmentDate)
        {
            Student = student;
            Course = course;
            EnrollmentDate = enrollmentDate.ToString();
        }

        public Course Course { get => _course; set => _course = value; }
        public string EnrollmentDate
        {
            get => _enrollmentDate.ToString("yyyy-MM-dd");
            set => _enrollmentDate = DateTime.Parse(value);
        }
        public Student Student { get => _student; set => _student = value; }
    }
}
