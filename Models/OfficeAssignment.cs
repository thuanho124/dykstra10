using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class OfficeAssignment
    {
        // define InstructorID as primary key and foreign key 
        [Key]
        public int InstructorID { get; set; }
        [StringLength(50)]
        [Display(Name = "Office Location")]
        public string Location { get; set; }

        // navigation property
        // Assign Instructor property to related OfficeAssignment property 
        public Instructor Instructor { get; set; }
    }
}