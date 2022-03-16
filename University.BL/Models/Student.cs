using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace University.BL.Models
{
    [Table("Student", Schema = "dbo")]
    public class Student
    {
        

        [Key]
        public int  ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        //REFERENCIAS

        public virtual IEnumerable<Enrollment> Enrollments { get; set; }
    }
}
