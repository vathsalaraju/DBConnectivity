using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace DBConnectivity
{
   

    public class InMemoryAppEngine// : AppEngine

    {
        List<Course> listOfCourses = new List<Course>();
        List<Enroll> listOfEnrollment = new List<Enroll>();
        List<Student> listOfStudents = new List<Student>();
        Enroll e = new Enroll();
        static int count = 0;


        public static SqlConnection conn;

        public InMemoryAppEngine()
        {
            string connectstr = ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectstr);
            conn.Open();
        }
        public void enroll(Student student, Course course, DateTime enrollmentDate)
        {
            Enroll e = new Enroll(student, course, enrollmentDate);
            try
            {
                string connectstr = ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString;
                SqlConnection conn = new SqlConnection(connectstr);
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into enrollcourse(sid,cid,enrollmentdate) values(@sid,@cid,@enrollmentdate)", conn);
                cmd.Parameters.AddWithValue("@sid", e.Student.Id);
                cmd.Parameters.AddWithValue("@cid", e.Course.Id);
                cmd.Parameters.AddWithValue("@enrollmentdate", e.EnrollmentDate);
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                    Console.WriteLine("Successfully inserted");
            }
            catch (SqlException e1)
            {
                Console.WriteLine(e1.Message);
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
           
            foreach (Enroll en in e.listOfEnrollments())
            {
                if (en.Student.Id == student.Id && en.Course.Id == course.Id)
                    throw new NoRepeatEnrollmentException("you have already registered for the course");
            }

            foreach (Enroll en in e.listOfEnrollments())
            {
                if (en.Student.Id == student.Id)
                    count++;
            }
            if (count > 4)
                throw new LimitRegistrationException("You have exceeded the number of registration");
            e.enrollList.Add(e);
        }

        public void register(Student student)
        {
            try
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                SqlCommand cmd = new SqlCommand("insert into student(id,name,DOB) values(@id,@name,@dob)", conn);
                cmd.Parameters.AddWithValue("@id", student.Id);
                cmd.Parameters.AddWithValue("@name", student.Name);
                cmd.Parameters.AddWithValue("@dob", student.Date);
                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                    Console.WriteLine("Successfully inserted");
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
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                if (course.IsDegree)
                {
                    isDegree = 1;
                    DegreeCourse dc = new DegreeCourse();
                    level = dc.level.ToString();
                    if (dc.isPlacementAvailable)
                        isplacementavailable = 1;
                    else
                        isplacementavailable = 0;
                    montlyfee = dc.calculateMonthlyFees();
                    cmd = new SqlCommand("insert into course(id,name,duration,fee,seatsAvailable,isDegree,degreelevel," +
                    "isplacementavailable,monthlyfee) values(@id,@name,@duration,@fee,@seatsAvailable,@isdegree," +
                    "@degreelevel,@isPlacementAvailable,@monthlyfee)", conn);
                    cmd.Parameters.AddWithValue("@id", course.Id);
                    cmd.Parameters.AddWithValue("@name", course.Name);
                    cmd.Parameters.AddWithValue("@duration", course.Duration);
                    cmd.Parameters.AddWithValue("@fee", course.Fee);
                    cmd.Parameters.AddWithValue("@seatsAvailable", course.SeatsAvailable);
                    cmd.Parameters.AddWithValue("@isdegree", isDegree);
                    cmd.Parameters.AddWithValue("@degreelevel", level);
                    cmd.Parameters.AddWithValue("@isPlacementAvailable", isplacementavailable);
                    cmd.Parameters.AddWithValue("@monthlyfee", montlyfee);
                }
                else
                {
                    DiplomaCourse dpc = new DiplomaCourse();
                    type = dpc.type.ToString();
                    montlyfee = dpc.calculateMonthlyFees();
                    cmd = new SqlCommand("insert into course(id,name,duration,fee,seatsAvailable,isDegree, DiplomaType,monthlyfee) values(@id,@name,@duration,@fee,@seatsAvailable,@isdegree,@diplomatype,@monthlyfee)", conn);
                    cmd.Parameters.AddWithValue("@id", course.Id);
                    cmd.Parameters.AddWithValue("@name", course.Name);
                    cmd.Parameters.AddWithValue("@duration", course.Duration);
                    cmd.Parameters.AddWithValue("@fee", course.Fee);
                    cmd.Parameters.AddWithValue("@seatsAvailable", course.SeatsAvailable);
                    cmd.Parameters.AddWithValue("@isdegree", isDegree);
                    cmd.Parameters.AddWithValue("@monthlyfee", montlyfee);
                    cmd.Parameters.AddWithValue("@diplomatype", type);
                }

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                    Console.WriteLine("Successfully inserted");
            }
            catch(SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            

        }
                        
    }
}

