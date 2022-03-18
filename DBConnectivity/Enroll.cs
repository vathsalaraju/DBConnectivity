using System.Configuration;
using System.Data.SqlClient;

namespace DBConnectivity
{
    public class Enroll
    {
        private Student _student;
        private Course _course;
        private DateTime _enrollmentDate;

        public List<Student> studentList = new List<Student>();
        public List<Course> courseList = new List<Course>();
        public List<Enroll> enrollList = new List<Enroll>();
       
       
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

        public List<Enroll> listOfEnrollments()
        {
            using(SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("select * from enrollcourse", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Enroll e1 = new Enroll();
                            string sid = (string)reader["sid"];
                            string cid = (string)reader["cid"];
                            e1.EnrollmentDate = reader["enrollmentdate"].ToString();
                            e1.Student = getStudentByid(sid);
                            e1.Course = getCourseByid(cid);
                            if(!enrollList.Contains(e1))
                                enrollList.Add(e1);
                        }
                    }

                }
            }           
            return enrollList;
        }


        public List<Course> listOfCourses()
        {

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("select * from course", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Course c;

                            if ((bool)reader["isDegree"])
                            {
                                DegreeCourse dc = new DegreeCourse();
                                dc.Id = (string)reader["id"];
                                dc.Name = (string)reader["name"];
                                dc.SeatsAvailable = (int)reader["seatsAvailable"];
                                dc.Fee = (float)(decimal)reader["fee"];
                                dc.Duration = (string)reader["Duration"];
                                if ((string)reader["degreelevel"] == "BACHELORS")
                                    dc.level = DegreeCourse.Level.BACHELORS;
                                else
                                    dc.level = DegreeCourse.Level.MASTERS;
                                dc.isPlacementAvailable = (bool)reader["isPlacementAvailable"];
                                if(!courseList.Contains(dc))
                                    courseList.Add(dc);
                            }
                            else
                            {
                                DiplomaCourse dpc = new DiplomaCourse();
                                dpc.Id = (string)reader["id"];
                                dpc.Name = (string)reader["name"];
                                dpc.SeatsAvailable = (int)reader["seatsAvailable"];
                                dpc.Fee = (float)reader["fee"];
                                dpc.Duration = (string)reader["Duration"];
                                if ((string)reader["DiplomaType"] == "PROFESSIONAL")
                                    dpc.type = DiplomaCourse.Type.PROFESSIONAL;
                                else
                                    dpc.type = DiplomaCourse.Type.ACADEMIC;
                                if (!courseList.Contains(dpc))
                                    courseList.Add(dpc);
                            }
                        }
                    }
                }
            }
            
            return courseList;
        }

        public List<Student> listOfStudents()
        {

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("select * from student", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            Student student = new Student();
                            student.Id = (string)reader["id"];
                            student.Name = (string)reader["name"];
                            student.Date = reader["DOB"].ToString();
                            if(!studentList.Contains(student))
                                studentList.Add(student);
                        }
                    }
                }
            }
            return studentList;
        }

        public Student getStudentByid(string id)
        {
            foreach (Student s in listOfStudents())
            {
                if (s.Id == id)
                    return s;
            }
            throw new Exception("student id not found");
        }
        public Course getCourseByid(string id)
        {
            foreach (Course c in listOfCourses())
            {
                if (c.Id == id)
                    return c;
            }
            throw new Exception("course id not found");
        }
    }
}
