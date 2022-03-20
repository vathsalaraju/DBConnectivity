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
        }
        public void enroll(Student student, Course course, DateTime enrollmentDate)
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
            e.enrollList.Add(e);
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

