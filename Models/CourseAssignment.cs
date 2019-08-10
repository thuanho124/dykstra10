using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class CourseAssignment
    {

        // InstructorID as a composite primary key, which connects Instructor entity with CourseAssignment and Course
        public int InstructorID { get; set; }

        // CourseID as a composite primary key, which connects Course entity with CourseAssignment and Instructor
        public int CourseID { get; set; }

        // navigation property
        public Instructor Instructor { get; set; }
        public Course Course { get; set; }
    }
}