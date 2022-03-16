using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using University.BL.Data;
using University.BL.DTOs;
using University.BL.Models;

namespace University.Web.Controllers
{
    public class OfficeAssignmentsController : Controller
    {
        private readonly DBContext context = new DBContext();
        [HttpGet]
        public ActionResult Create()
        {
            GetInstructors();
            return View();
        }
        [HttpPost]
        public ActionResult Create(OfficeAssigmentDTO officeAssigmentDTO)
        {
            GetInstructors();

            if (ModelState.IsValid)
            {

                var officeModel = new OfficeAssignment
                {
                    InstructorID = officeAssigmentDTO.InstructorID,
                    Location = officeAssigmentDTO.Location,
                    
                };
                context.OfficeAssignments.Add(officeModel);
                context.SaveChanges();  

            }
            return View(officeAssigmentDTO);
        }

        private void GetInstructors()
        {
              var instructorsModel = context.Instructors.ToList();    
            var instructorsDTO = instructorsModel.Select(x=> ConvertInstructor(x)).ToList();
            ViewBag.Instructors = new SelectList(instructorsDTO, "ID", "FullName");

        }
        private InstructorDTO ConvertInstructor(Instructor instructor)
        {
            return new InstructorDTO
            {
                ID = instructor.ID,
                LastName = instructor.LastName,
                FirstMidName = instructor.FirstMidName,
                HireDate = instructor.HireDate,
            };
        }
    }
}