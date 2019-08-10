using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models.SchoolViewModels
{
    // dykstra3
    // added a model for the about page
    public class EnrollmentDateGroup
    {
        [DataType(DataType.Date)]
        // prop for enrollment date
        public string YearRank { get; set; }

        // prop for student count
        public int StudentCount { get; set; }
    }
}