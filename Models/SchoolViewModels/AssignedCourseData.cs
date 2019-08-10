using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Models.SchoolViewModels
{
    public class AssignedCourseData
    {
        //prop to get Course ID
        public int CourseID { get; set; }

        // prop tio get Title
        public string Title { get; set; }

        // prop tio get Assigned
        public bool Assigned { get; set; }
    }
}