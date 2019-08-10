using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]  // maximun length = 50 and minimum length = 3 for Name field
        public string Name { get; set; }

        [DataType(DataType.Currency)] // Data type = currency, to define money type in SQL database
        [Column(TypeName = "money")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        // there may be an adminstrator for each department. An adminstrator is an instructor,
        // so InstructorID is a foreign key, connected to Instructor table. 
        public int? InstructorID { get; set; }

        // tracking property
        // this column will be inlcuded in Where clause sent to the database.
        [Timestamp]
        public byte[] RowVersion { get; set; }

        // navigation property Administrator holds an Instructor entity
        public Instructor Administrator { get; set; }

        // navigation property, that indicates one department can have many course. 
        public ICollection<Course> Courses { get; set; }
    }
}