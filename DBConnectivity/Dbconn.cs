using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DBConnectivity
{
    public class InMemoryAppEngine : AppEngine

    {
      
        public List<Student> studentList = new List<Student>();
        public List<Course> courseList = new List<Course>();
        public List<Enroll> enrollList = new List<Enroll>();
        Enroll e = new Enroll();
        static int count = 0;

        public InMemoryAppEngine()
        {
        }
        public void enrollCourse(Student student, Course course, DateTime enrollmentDate)
        {
            Enroll e = new Enroll(student, course, enrollmentDate);
            int result = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("insertEnroll", conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        checkNumberOfEnrollments(student.Id);
                        checkIfExists(student.Id, course.Id);
                        cmd.Parameters.AddWithValue("@sid", SqlDbType.NVarChar).Value = e.Student.Id;
                        cmd.Parameters.AddWithValue("@cid", SqlDbType.NVarChar).Value = e.Course.Id;
                        cmd.Parameters.AddWithValue("@enrollmentdate", SqlDbType.Date).Value = e.EnrollmentDate;
                        result = cmd.ExecuteNonQuery();
                        if(result>0)
                            Console.WriteLine("Successfully inserted");
                    }
                }
            }
            catch (SqlException e1)
            {
                Console.WriteLine(e1.Message);
                return;
                
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
                return;
                
            }
            enrollList.Add(e);
        }

        public void checkNumberOfEnrollments(string id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("select count(*) from enrollcourse where sid = @id", conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@id", id);
                        int result = Convert.ToInt32(cmd.ExecuteScalar());
                        Console.WriteLine(result);
                        if (result > 4)
                        {
                            throw new LimitRegistrationException("you have exceeded the maximum enrollments");
                        }

                    }
                }
            }
            catch(LimitRegistrationException e)
            {
                Console.WriteLine(e.Message);
                
            }
        }

        public void checkIfExists(string sid,string cid)
        {
            int result = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("select count(*) from enrollcourse where sid = @sid and cid = @cid", conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@sid", sid);
                        cmd.Parameters.AddWithValue("@cid", cid);
                        result = Convert.ToInt32(cmd.ExecuteScalar());
                        if (result > 0)
                        {
                            throw new NoRepeatEnrollmentException("you have already registered for the course");
                        }

                    }
                }
            }
            catch(NoRepeatEnrollmentException e)
            {
                Console.WriteLine(e.Message);
            }

        }
    
        public void register(Student student)
        {
            try
            {


                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("insertStudent", conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", SqlDbType.NVarChar).Value = student.Id;
                        cmd.Parameters.AddWithValue("@name", SqlDbType.NVarChar).Value = student.Name;
                        cmd.Parameters.AddWithValue("@dob", SqlDbType.Date).Value = student.Date;
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                            Console.WriteLine("Successfully inserted");
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            

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
                                int index = courseList.FindIndex(c => c.Id == dc.Id);
                                if (index < 0)
                                    courseList.Add(dc);
                            }
                            else
                            {
                                DiplomaCourse dpc = new DiplomaCourse();
                                dpc.Id = (string)reader["id"];
                                dpc.Name = (string)reader["name"];
                                dpc.SeatsAvailable = (int)reader["seatsAvailable"];
                                dpc.Fee = (float)(decimal)reader["fee"];
                                dpc.Duration = (string)reader["Duration"];
                                if ((string)reader["DiplomaType"] == "PROFESSIONAL")
                                    dpc.type = DiplomaCourse.Type.PROFESSIONAL;
                                else
                                    dpc.type = DiplomaCourse.Type.ACADEMIC;
                                int index = courseList.FindIndex(c => c.Id == dpc.Id);
                                if (index < 0)
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
                            int index = studentList.FindIndex(s => s.Id == student.Id);
                            if (index < 0)
                                studentList.Add(student);
                        }
                    }
                }
            }
            return studentList;
        }
        public List<Enroll> listOfEnrollments()
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString))
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
                            int index = enrollList.FindIndex(e => e.Student.Id == sid && e.Course.Id == cid);
                            if (index < 0)
                                enrollList.Add(e1);
                        }
                    }

                }
            }
            return enrollList;
        }

        public void introduce(Course course)
        {
            SqlCommand cmd;
            byte isDegree = 0;
            string level = null;
            byte? isplacementavailable = null;
            string type = null;
            double montlyfee;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString))
                {
                    using (cmd = new SqlCommand("insert into course(id,name,duration,fee,seatsAvailable,isDegree,degreelevel," +
                        "isplacementavailable,monthlyfee) values(@id,@name,@duration,@fee,@seatsAvailable,@isdegree," +
                        "@degreelevel,@isPlacementAvailable,@monthlyfee)", conn))
                    {
                        conn.Open();
                        if (course.IsDegree)
                        {
                            isDegree = 1;
                            DegreeCourse dc = new DegreeCourse();
                            level = dc.level.ToString();
                            dc.Fee = course.Fee;
                            if (dc.isPlacementAvailable)
                                isplacementavailable = 1;
                            else
                                isplacementavailable = 0;
                            

                            cmd.Parameters.AddWithValue("@id", course.Id);
                            cmd.Parameters.AddWithValue("@name", course.Name);
                            cmd.Parameters.AddWithValue("@duration", course.Duration);
                            cmd.Parameters.AddWithValue("@fee", course.Fee);
                            cmd.Parameters.AddWithValue("@seatsAvailable", course.SeatsAvailable);
                            cmd.Parameters.AddWithValue("@isdegree", isDegree);
                            cmd.Parameters.AddWithValue("@degreelevel", level);
                            cmd.Parameters.AddWithValue("@isPlacementAvailable", isplacementavailable);
                            cmd.Parameters.AddWithValue("@monthlyfee", dc.calculateMonthlyFees());
                        }
                        else
                        {
                            DiplomaCourse dpc = new DiplomaCourse();
                            type = dpc.type.ToString();
                            dpc.Fee = course.Fee;
                            
                            cmd = new SqlCommand("insert into course(id,name,duration,fee,seatsAvailable,isDegree, DiplomaType,monthlyfee) values(@id,@name,@duration,@fee,@seatsAvailable,@isdegree,@diplomatype,@monthlyfee)", conn);
                            cmd.Parameters.AddWithValue("@id", course.Id);
                            cmd.Parameters.AddWithValue("@name", course.Name);
                            cmd.Parameters.AddWithValue("@duration", course.Duration);
                            cmd.Parameters.AddWithValue("@fee", course.Fee);
                            cmd.Parameters.AddWithValue("@seatsAvailable", course.SeatsAvailable);
                            cmd.Parameters.AddWithValue("@isdegree", isDegree);
                            cmd.Parameters.AddWithValue("@monthlyfee", dpc.calculateMonthlyFees());
                            cmd.Parameters.AddWithValue("@diplomatype", type);
                        }

                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                            Console.WriteLine("Successfully inserted");
                    }
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            

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

