using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Instructor : Person
    {
        // datatype attribute used to specify a data type to a specific property
        [DataType(DataType.Date), Display(Name = "Hire Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HireDate { get; set; }


        // navigation property CourseAssignments entites related to Instructor entity
        public ICollection<CourseAssignment> CourseAssignments { get; set; }

        // navigation property that only holds a single entity
        public OfficeAssignment OfficeAssignment { get; set; }
    }
}