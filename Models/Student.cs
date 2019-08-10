using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Student : Person
    {
        
        [Required] // make the property a required field
        [StringLength(50)]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required] // make the property a required field
        [StringLength(10)]
        [Display(Name = "Year Rank")]
        public string YearRank { get; set; }

        // make sure users input grade from 0.0 to 4.0. Nothing more 4.0 or less than 0.0
        [Range(0,4)]
        [Display(Name = "GPA")]
        public float AverageGrade { get; set; }

        // datatype attribute used to specify a datatype
        [DataType(DataType.Date)]
        // Format the date to yyyy-MM-dd (year-month-day)
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment  Date")]
        public DateTime EnrollmentDate { get; set; }

        

         
              
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
