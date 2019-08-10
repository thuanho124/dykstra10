using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public abstract class Person
    {
        public int ID { get; set; }

        [Required] // required field
        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters.")] // string length = 50
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required] // Required field
        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters.")]
        [Column("FirstName")] // display column in database 
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        // get full name 
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }
    }
}