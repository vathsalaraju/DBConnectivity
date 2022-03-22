using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBConnectivity
{
    public class Info
    {
        public static void Display(Student student)
        {

            Console.WriteLine("Student ID : "+student.Id + "\tStudentName : " + student.Name + "\tDate of Birth : " + student.Date);
        }

        public void Display(Course course)
        {
            Console.WriteLine("Course ID : "+course.Id + "\tCourse Name : " + course.Name + "\tCourse Fee : " + course.Fee + "\t Duration : " + course.Duration + "\tMonthlyFee : " + course.calculateMonthlyFees()+"\t Seats Available : "+course.SeatsAvailable);
        }

        public void Display(Enroll enroll)
        {

            Console.WriteLine("Student ID : "+enroll.Student.Id+"\tStudent Name : "+enroll.Student.Name+"\tCourse ID : "+enroll.Course.Id+"\t Course Name : "+enroll.Course.Name+"\tDuration : "+enroll.Course.Duration+"\tEnrollment Date : "+enroll.EnrollmentDate);
            
        }

    }
}
