using Magistri.DTO;
using Magistri.Models;
using Magistri.Services;
using Magistri.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Magistri.Controllers
{
    [Authorize(Roles = "Student, Teacher, Admin")]
    public class GradesController : Controller
    {
        private GradeService gradeService;
        public GradesController(GradeService gradeService)
        {
            this.gradeService = gradeService;
        }
        public IActionResult Index()
        {
            var allGrades = gradeService.GetAllGrades();
            return View(allGrades);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var gradesDropdownData = gradeService.GetNewGradesDropdownsValues();
            ViewBag.Students = new SelectList(gradesDropdownData.Students, "Id", "LastName");
            ViewBag.Subjects = new SelectList(gradesDropdownData.Subjects, "Id", "Name");
            return View();
        }
        [HttpPost]
        public IActionResult Create(GradeDTO gradeDTO)
        {
            if (gradeDTO == null)
            {
                return BadRequest("GradeDTO cannot be null.");
            }
            gradeService.Create(gradeDTO);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var grade = gradeService.GetById(id);
            if (grade == null) { return View("NotFound"); }
            var gradesViewModel = new GradeDTO()
            {
                Id = grade.Id,
                StudentId = grade.Student.Id,
                SubjectId = grade.Subject.Id,
                Title = grade.Title,
                Mark = grade.Mark,
                Date = grade.Date
            };
            var gradesDropdownsValues = gradeService.GetNewGradesDropdownsValues();
            ViewBag.Students = new SelectList(gradesDropdownsValues.Students, "Id", "LastName");
            ViewBag.Subjects = new SelectList(gradesDropdownsValues.Subjects, "Id", "Name");
            return View(gradesViewModel);
        }
        [HttpPost]
        public IActionResult Edit(int id, GradeDTO grade)
        {
            gradeService.Update(id, grade);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            gradeService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
