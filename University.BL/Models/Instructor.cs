using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
namespace University.BL.Models
{
    [Table("Instructor", Schema = "dbo")]
    public class Instructor
    {
        
        [Key]
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime HireDate { get; set; }
        public virtual OfficeAssignment OfficeAssignment  { get; set; }
        public virtual IEnumerable<CourseInstructor> CourseInstructors { get; set; }
    }
}
    

