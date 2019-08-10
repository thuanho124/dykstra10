using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContosoUniversity.Data
{
    public class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            // sample data for Student
            var students = new Student[]
            {
                    new Student{FirstName="Carson",LastName="Alexander",EmailAddress="caralex2@uw.edu",YearRank ="Junior", AverageGrade = 3.4f, EnrollmentDate=DateTime.Parse("2005-09-01")},
                    new Student{FirstName="Meredith",LastName="Alonso",EmailAddress="mealo13@uw.edu",YearRank ="Junior", AverageGrade = 2.9f, EnrollmentDate=DateTime.Parse("2002-09-01")},
                    new Student{FirstName="Arturo",LastName="Anand",EmailAddress="anaart9@uw.edu",YearRank ="Sophomore", AverageGrade = 3.9f, EnrollmentDate=DateTime.Parse("2003-09-01")},
                    new Student{FirstName="Gytis",LastName="Barzdukas",EmailAddress="barzd4@uw.edu",YearRank ="Freshmen", AverageGrade = 3.5f, EnrollmentDate=DateTime.Parse("2002-09-01")},
                    new Student{FirstName="Yan",LastName="Li",EmailAddress="liyan8@uw.edu",YearRank ="Senior", AverageGrade = 2.3f, EnrollmentDate=DateTime.Parse("2002-09-01")},
                    new Student{FirstName="Peggy",LastName="Justice",EmailAddress="peggy2@uw.edu",YearRank ="Junior", AverageGrade = 3.2f, EnrollmentDate=DateTime.Parse("2001-09-01")},
                    new Student{FirstName="Laura",LastName="Norman",EmailAddress="norlau10@uw.edu",YearRank ="Freshmen", AverageGrade = 3.1f, EnrollmentDate=DateTime.Parse("2003-09-01")},
                    new Student{FirstName="Nino",LastName="Olivetto",EmailAddress="olive6@uw.edu",YearRank ="Senior", AverageGrade = 4.0f, EnrollmentDate=DateTime.Parse("2005-09-01")}
            };
            foreach (Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();


            // sample data for Instructor 
            var instructors = new Instructor[]
           {
                new Instructor { FirstName = "Kim",     LastName = "Abercrombie",
                    HireDate = DateTime.Parse("1995-03-11") },
                new Instructor { FirstName = "Fadi",    LastName = "Fakhouri",
                    HireDate = DateTime.Parse("2002-07-06") },
                new Instructor { FirstName = "Roger",   LastName = "Harui",
                    HireDate = DateTime.Parse("1998-07-01") },
                new Instructor { FirstName = "Candace", LastName = "Kapoor",
                    HireDate = DateTime.Parse("2001-01-15") },
                new Instructor { FirstName = "Roger",   LastName = "Zheng",
                    HireDate = DateTime.Parse("2004-02-12") },
                new Instructor { FirstName = "John",   LastName = "David",
                    HireDate = DateTime.Parse("2004-03-10") }
           };

            foreach (Instructor i in instructors)
            {
                context.Instructors.Add(i);
            }
            context.SaveChanges();


            // sample data for Department
            var departments = new Department[]
            {
                new Department { Name = "English",     Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Abercrombie").ID },
                new Department { Name = "Mathematics", Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Fakhouri").ID },
                new Department { Name = "Engineering", Budget = 350000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Harui").ID },
                new Department { Name = "Economics",   Budget = 100000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Kapoor").ID },
                new Department { Name = "Information Technology",   Budget = 200000,
                    StartDate = DateTime.Parse("2007-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "David").ID }
            };

            foreach (Department d in departments)
            {
                context.Departments.Add(d);
            }
            context.SaveChanges();

            // sample data for Course
            var courses = new Course[]
            {
            new Course{CourseID=1050,Title="Chemistry",Credits=3, RoomNum="BHS 409",
                    DepartmentID = departments.Single( s => s.Name == "Engineering").DepartmentID},
            new Course{CourseID=4022,Title="Microeconomics",Credits=3, RoomNum="CP 503",
                    DepartmentID = departments.Single( s => s.Name == "Economics").DepartmentID},
            new Course{CourseID=4041,Title="Macroeconomics",Credits=3, RoomNum="WG 689",
                    DepartmentID = departments.Single( s => s.Name == "Economics").DepartmentID},
            new Course{CourseID=1045,Title="Calculus",Credits=4, RoomNum="GWP 110",
                    DepartmentID = departments.Single( s => s.Name == "Mathematics").DepartmentID},
            new Course{CourseID=3141,Title="Trigonometry",Credits=4, RoomNum="MAT 941",
                    DepartmentID = departments.Single( s => s.Name == "Mathematics").DepartmentID},
            new Course{CourseID=2021,Title="Composition",Credits=3, RoomNum="WCG 001",
                    DepartmentID = departments.Single( s => s.Name == "English").DepartmentID},
            new Course{CourseID=2042,Title="Literature",Credits=4, RoomNum="BB 276",
                    DepartmentID = departments.Single( s => s.Name == "English").DepartmentID},
            new Course{CourseID=2078,Title="ASP.NET MVC Web App",Credits=4, RoomNum="BB 276",
                    DepartmentID = departments.Single( s => s.Name == "English").DepartmentID}
            };
            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            // sample data for Enrollment
            var enrollments = new Enrollment[]
            {
                new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").ID,
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID,
                    Grade = Grade.A
                },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").ID,
                    CourseID = courses.Single(c => c.Title == "Microeconomics" ).CourseID,
                    Grade = Grade.C
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").ID,
                    CourseID = courses.Single(c => c.Title == "Macroeconomics" ).CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                        StudentID = students.Single(s => s.LastName == "Alonso").ID,
                    CourseID = courses.Single(c => c.Title == "Calculus" ).CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                        StudentID = students.Single(s => s.LastName == "Alonso").ID,
                    CourseID = courses.Single(c => c.Title == "Trigonometry" ).CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alonso").ID,
                    CourseID = courses.Single(c => c.Title == "Composition" ).CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Anand").ID,
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Anand").ID,
                    CourseID = courses.Single(c => c.Title == "Microeconomics").CourseID,
                    Grade = Grade.B
                    },
                new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Barzdukas").ID,
                    CourseID = courses.Single(c => c.Title == "Chemistry").CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Li").ID,
                    CourseID = courses.Single(c => c.Title == "Composition").CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Justice").ID,
                    CourseID = courses.Single(c => c.Title == "Literature").CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Olivetto").ID,
                    CourseID = courses.Single(c => c.Title == "Microeconomics").CourseID,
                    Grade = Grade.A
                    }
            };

            foreach (Enrollment e in enrollments)
            {
                var enrollmentInDataBase = context.Enrollments.Where(
                    s =>
                            s.Student.ID == e.StudentID &&
                            s.Course.CourseID == e.CourseID).SingleOrDefault();
                if (enrollmentInDataBase == null)
                {
                    context.Enrollments.Add(e);
                }
            }
            context.SaveChanges();


            // sample data for Office Assignment
            var officeAssignments = new OfficeAssignment[]
            {
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.LastName == "Fakhouri").ID,
                    Location = "Smith 17" },
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.LastName == "Harui").ID,
                    Location = "Gowan 27" },
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.LastName == "Kapoor").ID,
                    Location = "Thompson 304" },
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.LastName == "David").ID,
                    Location = "MAT 304" },
            };

            foreach (OfficeAssignment o in officeAssignments)
            {
                context.OfficeAssignments.Add(o);
            }
            context.SaveChanges();


            // sample data for Course Assignment
            var courseInstructors = new CourseAssignment[]
           {
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID,
                    InstructorID = instructors.Single(i => i.LastName == "Kapoor").ID
                    },
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID,
                    InstructorID = instructors.Single(i => i.LastName == "Harui").ID
                    },
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Microeconomics" ).CourseID,
                    InstructorID = instructors.Single(i => i.LastName == "Zheng").ID
                    },
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Macroeconomics" ).CourseID,
                    InstructorID = instructors.Single(i => i.LastName == "Zheng").ID
                    },
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Calculus" ).CourseID,
                    InstructorID = instructors.Single(i => i.LastName == "Fakhouri").ID
                    },
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Trigonometry" ).CourseID,
                    InstructorID = instructors.Single(i => i.LastName == "Harui").ID
                    },
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Composition" ).CourseID,
                    InstructorID = instructors.Single(i => i.LastName == "Abercrombie").ID
                    },
                new CourseAssignment {
                    CourseID = courses.Single(c => c.Title == "Literature" ).CourseID,
                    InstructorID = instructors.Single(i => i.LastName == "Abercrombie").ID
                    },
           };

            foreach (CourseAssignment ci in courseInstructors)
            {
                context.CourseAssignments.Add(ci);
            }
            context.SaveChanges();

        }
    }
}