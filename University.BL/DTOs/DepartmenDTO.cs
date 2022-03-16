using System;
using System.ComponentModel.DataAnnotations;

namespace University.BL.DTOs
{
    public class DepartmenDTO
    {
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "The field Last Name is required")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field FirstMid Name is required")]
        [StringLength(50)]
        [Display(Name = "FirstMid Name")]
        public decimal Budget { get; set; }

        [Required(ErrorMessage = "The field HireDate is required")]
        [Display(Name = "HireDate")]
        public DateTime StartDate { get; set; }

        public int InstructorID { get; set; }

    }
}
