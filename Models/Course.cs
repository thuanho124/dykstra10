using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ContosoUniversity.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // primary key values are provided by user. No values will be automatically generated in database
        [Display(Name = "Course #ID")] // change the display for text box to Course #ID with space
        public int CourseID { get; set; }

        [StringLength(50, MinimumLength = 3)] // maximun length = 50 and minimum length = 3 for Title field
        public string Title { get; set; }

        [Range(0,5)]
        public int Credits { get; set; }

        [StringLength(12, MinimumLength = 7)] // maximun length = 12 and minimum length = 7 for RoomNum field
        public string RoomNum { get; set; }

        // points to the related Department entity in order to update things easily in the database
        public int DepartmentID { get; set; }

        // navigation property
        // Assign one Course property to one related Department property
        public Department Department { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }

        // navigation property indicates that:
        // a course can be taught by many different instructors
        public ICollection<CourseAssignment> CourseAssignments { get; set; }
    }
}