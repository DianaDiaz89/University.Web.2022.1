using System;
using System.Collections.Generic;
using System.Web.Mvc;
using University.BL.DTOs;
using System.Linq;
using University.BL.Data;
using University.BL.Models;



namespace University.Web.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly DBContext context = new DBContext();
        private static List<InstructorDTO> instructors = new List<InstructorDTO>();



        // Instructors/Index
        // GET: Instructors
        [HttpGet]
        public ActionResult Index(int ? id)
        {
            #region TraspasoDato
            //1. View(model);
            //2. ViewBag
            //3. ViewData
            //4. Session



            ViewBag.InstructorsCount = instructors.Count;
            ViewBag.InstructorFirst = instructors.FirstOrDefault();



            ViewData["InstructorsCount"] = instructors.Count;
            ViewData["InstructorFirst"] = instructors.FirstOrDefault();



            Session["InstructorsCount"] = instructors.Count;
            var count = (int)Session["InstructorsCount"];
            Session.Remove("InstructorsCount");
            Session.RemoveAll();
            #endregion



            //esto es lo mismo que hacer un select en sql
            var instructorsModel = context.Instructors.ToList();
            var instructorsDTO = instructorsModel.Select(x => ConvertInstructor(x)).ToList();

            if (id != null)
            {// siempre foranea a primaria
                var coursesModel = (from _courseinstructors in context.CourseInstructors
                                    join _courses in context.Courses
                                    on _courseinstructors.CourseID equals _courses.CourseID
                                    where _courseinstructors.InstructorID == id.Value
                                    select _courses).ToList();

                var coursesDTO = coursesModel.Select(x => ConvertCourse(x)).ToList();


                ViewBag.Courses = coursesDTO;
            }

            return View(instructorsDTO);
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
        private CourseDTO ConvertCourse(Course courset)
        {
            return new CourseDTO
            {
                CourseID = courset.CourseID,
                Title = courset.Title,
                Credits = courset.Credits,



            };
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Create(InstructorDTO instructorDTO)
        {



            try
            {
                if (ModelState.IsValid)
                {
                    // Entity Framework
                    var instructor = new Instructor
                    {
                        LastName = instructorDTO.LastName,
                        FirstMidName = instructorDTO.FirstMidName,
                        HireDate = instructorDTO.HireDate,
                    };
                    context.Instructors.Add(instructor);
                    context.SaveChanges();



                    instructors.Add(instructorDTO);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(instructorDTO);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var instructorModel = context.Instructors.Find(id);
            var instructorDTO = ConvertInstructor(instructorModel);



            return View(instructorDTO);
        }



        [HttpPost]
        public ActionResult Edit(InstructorDTO instructorDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var instructorModel = context.Instructors.Find(instructorDTO.ID);
                    instructorModel.LastName = instructorDTO.LastName;
                    instructorModel.FirstMidName = instructorDTO.FirstMidName;
                    instructorModel.HireDate = instructorDTO.HireDate;



                    context.SaveChanges();



                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }



            return View(instructorDTO);
        }



        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                // dependencias
                var OfficeAssignments = context.OfficeAssignments.Where(x => x.InstructorID == id).ToList();
                if (!OfficeAssignments.Any())
                {
                    var instructorModel = context.Instructors.Find(id);
                    context.Instructors.Remove(instructorModel);
                    context.SaveChanges();
                }
                else
                    throw new Exception("Tiene oficina asignada");



            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }



    }
}