using System.Configuration;
using System.Data.SqlClient;

namespace DBConnectivity
{
    
    class ScreenDescription : UserInterface
    {
        Enroll en = new Enroll();
        Info info = new Info();
        InMemoryAppEngine app = new InMemoryAppEngine();
        


        public void introduceNewCourseScreen()
        {
            string connectstr = ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectstr);
            conn.Open();
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();

            System.Console.WriteLine("You are in new Course Screen");
            System.Console.WriteLine("Add new Course screen");
            System.Console.WriteLine("Enter course id");
            string courseId = Console.ReadLine();
            System.Console.WriteLine("Enter Course Name");
            string courseName = Console.ReadLine();
            System.Console.WriteLine("Enter Course duration");
            string duration = Console.ReadLine();
            System.Console.WriteLine("Enter Course Fee");
            float courseFee = float.Parse(Console.ReadLine());
            System.Console.WriteLine("Enter number of Seats Available");
            int seats = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter type Degree/Diploma");
            string type = Console.ReadLine();
            if (type == "Degree")
            {
                
                System.Console.WriteLine("Enter degree type - Bachelors/Masters");
                string dType = Console.ReadLine();
                System.Console.WriteLine("Is placement Available True/False");
                bool placement = Convert.ToBoolean(Console.ReadLine());
                if (dType == "Bachelors")
                {
                    app.introduce(new DegreeCourse(courseId, courseName, duration, courseFee, seats, true, placement, DegreeCourse.Level.BACHELORS));
                }
                else
                {
                    app.introduce(new DegreeCourse(courseId, courseName, duration, courseFee, seats, true,placement, DegreeCourse.Level.MASTERS));
                }
            }
            else
            {
                System.Console.WriteLine("Enter Diploma type Professional/Academic");
                string dtype = Console.ReadLine();
                if (dtype == "Professional")
                {
                    app.introduce(new DiplomaCourse(courseId, courseName, duration, courseFee, seats, false, DiplomaCourse.Type.PROFESSIONAL));
                }
                else
                {
                    app.introduce(new DiplomaCourse(courseId, courseName, duration, courseFee, seats, false, DiplomaCourse.Type.ACADEMIC));
                }
            }
            
        }

        public void showAdminScreen()
        {
            
            System.Console.WriteLine("===========================================");
            System.Console.WriteLine("You are in Admin Screen");
            System.Console.WriteLine("===========================================");
            System.Console.WriteLine("Enter your choice \n1. Show all students details \n2. show all  student enrollment\n3. Introduce new course\n4. show all courses\n");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1: 
                    showAllStudentScreen();
                    break;

                case 2: showAllEnrollment();
                    break;

                case 3: introduceNewCourseScreen();
                    break;

                case 4: showAllCoursesScreen();
                    break;

                default: System.Console.WriteLine("Invalid Options");
                    break;

            }
            
        }

        public void showAllEnrollment()
        {
            List<Enroll> e = new List<Enroll>();
            e = app.listOfEnrollments();
            
            if (e.Count() != 0)
            {
                System.Console.WriteLine("===========================================");
                System.Console.WriteLine("Displaying Enrollment Details");
                System.Console.WriteLine("===========================================\n\n");
                
                foreach (Enroll enr in e)
                {
                    info.Display(enr);
                }
            }
            else
            {
                System.Console.WriteLine("no enrollments found. please add records");
            }
            
        }

        public void showAllStudentScreen()
        {
           List<Student> s = new List<Student>();
            s = app.listOfStudents();
            
            if (s.Count() != 0) {
                System.Console.WriteLine("===========================================");
                System.Console.WriteLine("Displaying All Student Details");
                System.Console.WriteLine("===========================================\n\n");
                

                foreach (Student student in s)
                {
                    Info.Display(student);
                }
            }
            else
                System.Console.WriteLine("no students record to display please add records ");
        }

        public void showAllCoursesScreen()
        {
            List <Course> c = app.listOfCourses();

            
            if (c.Count() != 0)
            {
                System.Console.WriteLine("===========================================");
                System.Console.WriteLine("you are in Show all Courses Screen");
                System.Console.WriteLine("===========================================\n\n");
                
                foreach (Course course in c)
                {
                    info.Display(course);
                }
            }
            else {
                System.Console.WriteLine("No courses to display please add records");
            }
            
        }

        public void showFirstScreen()
        {
            
            System.Console.WriteLine("=====================================");
            System.Console.WriteLine("Welcome to Student Management System");
            System.Console.WriteLine("=====================================");
            System.Console.WriteLine("1. Admin\n2. Student\n3. Exit");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1: showAdminScreen();
                    break;
                case 2: showStudentScreen();
                    break;
                case 3: Environment.Exit(0);
                    break;

                default: System.Console.WriteLine("invalid option");
                    break;
            }
            
        }

        public void showStudentRegisterationScreen()
        {
            
            System.Console.WriteLine("===========================================");
            System.Console.WriteLine("You are in Student Registration Screen");
            System.Console.WriteLine("===========================================");
            try
            {
                System.Console.WriteLine("Enter student id");
                string sID = Console.ReadLine();
                System.Console.WriteLine("Enter student name");
                string sName = Console.ReadLine();
                System.Console.WriteLine("Enter date of birth(MM/DD/YYYY)");
                string date = Console.ReadLine();
                app.register(new Student(sID, sName, date));
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
           
        }

        public void showStudentScreen()
        {
            
            System.Console.WriteLine("===========================================");
            System.Console.WriteLine("You are in Student Screen");
            System.Console.WriteLine("===========================================");
            System.Console.WriteLine("Enter your choice:\n1. Register as student \n2. show Current student enrollments\n3. Show all student screen \n4. Enroll for course");
            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1: showStudentRegisterationScreen();
                    break;

                case 2:
                    Console.WriteLine("Enter student id");
                    string id = Console.ReadLine();
                    showSpecificStudentEnrollment(id);
                    break;

                case 3: showAllStudentScreen();
                    break;

                case 4: AddEnrollmentScreen();
                    break;

                default: System.Console.WriteLine("invalid choice");
                    break;
            }
            
        }
        
        public void showSpecificStudentEnrollment(string id)
        {
            Console.WriteLine("==================================================================================");
            Console.WriteLine("Student ID\t Course ID \t Date of Birth");
            Console.WriteLine("==================================================================================");
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstr"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("select * from enrollcourse where sid = @sid;", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@sid", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine((string)reader["sid"] +"\t"+(string)reader["cid"]+"\t"+reader["enrollmentdate"].ToString());
                            //Enroll en = new Enroll();
                            //en.Student.Id = (string)reader["sid"];
                            //en.Course.Id = (string)reader["cid"];
                            //en.EnrollmentDate = reader["enrollmentdate"].ToString();
                            //Console.WriteLine(en.Student.Id+"\t"+en.Course.Id+"\t"+en.EnrollmentDate);
                        }
                    }
                }
            }
        }

        public void AddEnrollmentScreen()
        {
            try
            {
                List<Course> courses = new List<Course>();
                courses = app.listOfCourses();
                List<Student> students = new List<Student>();
                students = app.listOfStudents();
                
                Student stud = new Student();
                System.Console.WriteLine("Enter student id");
                string id = Console.ReadLine();
                foreach (Student s in students)
                {
                    if (s.Id == id)
                    {
                        stud.Id = s.Id;
                        stud.Name = s.Name;
                        stud.Date = s.Date;
                        break;
                    }
                }
                foreach(Course c in courses)
                {
                    info.Display(c);
                }
                

                System.Console.WriteLine("Enter course id");
                string cour = Console.ReadLine();
                System.Console.WriteLine("Enter enrollment date");
                DateTime enrollDate = DateTime.Parse(Console.ReadLine());
                DegreeCourse dc;
                DiplomaCourse dpc;
                foreach (Course c in app.listOfCourses())
                {
                    if (cour == (string)c.Id)
                    {

                        if (c.GetType() == typeof(DegreeCourse))
                        {
                            DegreeCourse d = (DegreeCourse)c;
                            dc = new DegreeCourse();
                            dc.Id = c.Id;
                            dc.Name = c.Name;
                            dc.Duration = c.Duration;
                            dc.Fee = c.Fee;
                            dc.SeatsAvailable = c.SeatsAvailable;
                            dc.IsDegree = c.IsDegree;
                            dc.level = d.level;
                            dc.isPlacementAvailable = d.isPlacementAvailable;
                            app.enrollCourse(stud, dc, enrollDate);

                        }
                        else

                        {
                            DiplomaCourse d = (DiplomaCourse)c;
                            dpc = new DiplomaCourse();
                            dpc.Id = c.Id;
                            dpc.Name = c.Name;
                            dpc.Duration = c.Duration;
                            dpc.Fee = c.Fee;
                            dpc.SeatsAvailable = c.SeatsAvailable;
                            dpc.IsDegree = c.IsDegree;
                            dpc.type = d.type;

                            app.enrollCourse(stud, dpc, enrollDate);
                        }
                                        
                       }
                }
            }
            catch (FormatException e)
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
