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


    public class InMemoryAppEngine// : AppEngine

    {
        List<Course> listOfCourses = new List<Course>();
        List<Enroll> listOfEnrollment = new List<Enroll>();
        List<Student> listOfStudents = new List<Student>();
        Enroll e = new Enroll();
        static int count = 0;

        public InMemoryAppEngine()
        {
            //string connectstr = ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString;
            //SqlConnection conn = new SqlConnection(connectstr);
            //conn.Open();
        }
        public void enroll(Student student, Course course, DateTime enrollmentDate)
        {
            Enroll e = new Enroll(student, course, enrollmentDate);
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("insertEnroll", conn))
                    {
                        conn.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@sid", SqlDbType.NVarChar).Value = e.Student.Id;
                        cmd.Parameters.AddWithValue("@cid", SqlDbType.NVarChar).Value = e.Course.Id;
                        cmd.Parameters.AddWithValue("@enrollmentdate", SqlDbType.Date).Value = e.EnrollmentDate;
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                            Console.WriteLine("Successfully inserted");
                    }
                }
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
                        
    }
}

