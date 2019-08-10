using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }
    public class Enrollment
    {
        public int EnrollmentID { get; set; }

        // StudentID as a foreign key, which connects Student entity with Enrollment and Course
        public int StudentID { get; set; }

        // CourseID as a foreign key, which connects Course entity with Enrollment and Student
        public int CourseID { get; set; }

        [DisplayFormat(NullDisplayText = "No Grade")] // if grade is null or unavailable, the grade is displayed as "No Grade"
        public Grade? Grade { get; set; }

        // navigation property indicates that one student can have an enrollment record
        public Student Student { get; set; }

        // navigation property indicates that one course can have an enrollment record as well
        public Course Course { get; set; }
    }
}