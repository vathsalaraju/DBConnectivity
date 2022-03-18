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

            Console.WriteLine(student.Id + "\t" + student.Name + "\t" + student.Date);
        }

        public void Display(Course course)
        {
            Console.WriteLine(course.Id + "\t" + course.Name + "\t" + course.Fee + "\t" + course.Duration + "\t\t" + course.calculateMonthlyFees()+"\t\t"+course.SeatsAvailable);
        }

        public void Display(Enroll enroll)
        {

            Console.WriteLine(enroll.Student.Id+"\t\t"+enroll.Student.Name+"\t"+enroll.Course.Id+"\t\t"+enroll.Course.Name+"\t"+enroll.Course.Fee+"\t\t"+enroll.Course.Duration+"\t"+enroll.EnrollmentDate);
            
        }

    }
}
