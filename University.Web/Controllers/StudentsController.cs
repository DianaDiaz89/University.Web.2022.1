using System.Web.Mvc;
using University.BL.DTOs;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data.Entity;
using University.BL.Data;
using University.BL.Models;

namespace University.Web.Controllers
{
    
    public class StudentsController : Controller
    {
        private readonly DBContext context = new DBContext();

        private static List<StudentDTO> students = new List<StudentDTO> { 
       new StudentDTO{ID = 0, FirstMidName = "",LastName = "",EnrollmentDate = DateTime.UtcNow},
        new StudentDTO{ID = 2, FirstMidName = "Luis",LastName = "Santafe",EnrollmentDate = DateTime.UtcNow},
        new StudentDTO{ID = 3, FirstMidName = "Pedro",LastName = "Santafe",EnrollmentDate = DateTime.UtcNow}
        
        };
        // Students/Index
        // GET: Students
        [HttpGet]
        public ActionResult Index(int?id)
        {


            #region traspaso de datos
            ViewBag.StudentsCount = students.Count;
            ViewBag.StudentsFirst = students.FirstOrDefault();
            #endregion

            
            var studentsModel = context.Students.ToList();// select * from Students
            var studentsDTO = studentsModel.Select(x=> ConvertStudent(x)).ToList();

            if (id != null)
            {// siempre foranea a primaria
                var coursesModel = (from _enrollments in context.Enrollments
                               join _courses in context.Courses
                               on _enrollments.CourseID equals _courses.CourseID
                               where _enrollments.StudentID == id.Value
                               select _courses).ToList();

                var coursesDTO = coursesModel.Select(x => ConvertCourse(x)).ToList();
                ViewBag.Courses = coursesDTO;  
            }

            return View(studentsDTO);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(StudentDTO studentDTO)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    //EF

                    var student = new Student {
                    FirstMidName = studentDTO.FirstMidName,
                    LastName = studentDTO.LastName, 
                    EnrollmentDate = studentDTO.EnrollmentDate,
                    };

                    context.Students.Add(student);
                    context.SaveChanges();

                    students.Add(studentDTO);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(studentDTO);
        }
        private StudentDTO ConvertStudent(Student student)
        {
            return new StudentDTO
            {
                ID = student.ID,
                FirstMidName = student.FirstMidName,
                LastName = student.LastName,
                EnrollmentDate = student.EnrollmentDate
            };
        }
        private CourseDTO ConvertCourse(Course course)
        {
            return new CourseDTO
            {
               CourseID =  course.CourseID,
               Title = course.Title,    
               Credits = course.Credits,    
            };
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var studentModel = context.Students.Find(id);
            var studentDTO = ConvertStudent(studentModel);  
            return View(studentDTO);
        }
        [HttpPost]
        public ActionResult Edit(StudentDTO studentDTO)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    var studentModel = context.Students.Find(studentDTO.ID);
                    //campos a modificar 
                    //UPDATE Student SET = FirstMindName = FirstMindName
                    //WHERE id = @ID
                    studentModel.FirstMidName = studentDTO.FirstMidName;    
                    studentModel.LastName = studentDTO.LastName;
                    studentModel. EnrollmentDate = studentDTO.EnrollmentDate;

                    context.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(studentDTO);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                //dependencias
                var enrollments = context.Enrollments.Where(x=> x.StudentID == id).ToList();
                if (!enrollments.Any())
                {

                    var studentModel = context.Students.Find(id);
                    context.Students.Remove(studentModel);
                    context.SaveChanges();
                }
                else
                    throw new Exception("Tiene cursos matriculados.");
            }
            catch (Exception ex)
            {
                var message = ex.Message;
               
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
