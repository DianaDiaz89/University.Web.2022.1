using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using University.BL.Data;
using University.BL.DTOs;
using University.BL.Models;

namespace University.Web.Controllers
{
    public class CoursesController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Courses
        public ActionResult Index()
        {
            var coursesModel = db.Courses.ToList();
            var coursesDTO = coursesModel.Select(x => ConvertCourse(x)).ToList();
            return View(coursesDTO);
        }
        private CourseDTO ConvertCourse(Course course)
        {
            return new CourseDTO
            {
                CourseID = course.CourseID,
                Title = course.Title,
                Credits = course.Credits,
            };
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var courseModel = db.Courses.Find(id);
            if (courseModel == null)
            {
                return HttpNotFound();
            }
            var courseDTO = ConvertCourse(courseModel); 
            return View(courseDTO);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CourseID,Title,Credits")] CourseDTO courseDTO)
        {
            if (ModelState.IsValid)
            {
                var courseModel = new Course
                {

                    CourseID = courseDTO.CourseID,
                    Title= courseDTO.Title,
                    Credits= courseDTO.Credits, 
                };

                db.Courses.Add(courseModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(courseDTO);
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var courseModel = db.Courses.Find(id);
            if (courseModel == null)
            {
                return HttpNotFound();
            }
            var courseDTO = ConvertCourse(courseModel);
            return View(courseDTO);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseID,Title,Credits")] CourseDTO courseDTO)
        {
            if (ModelState.IsValid)
            {
                var courseModel = db.Courses.Find(courseDTO.CourseID);
                courseModel.Title = courseDTO.Title;
                courseModel.Credits = courseDTO.Credits;    

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(courseDTO);
        }

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var courseModel = db.Courses.Find(id);
            if (courseModel == null)
            {
                return HttpNotFound();
            }
            var courseDTO = ConvertCourse(courseModel);
            return View(courseDTO);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
