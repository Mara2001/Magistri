using Magistri.Models;
using Magistri.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Magistri.Controllers
{
    [Authorize(Roles = "Teacher, Admin")]
    public class StudentsController : Controller
    {
        private StudentService studentService;
        public StudentsController(StudentService service) {
            studentService = service;
        }       
        public IActionResult Index()
        {
            var allStudents = studentService.GetAllStudents();
            return View(allStudents);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(StudentDTO studentDTO)
        {            
            studentService.Create(studentDTO);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = studentService.GetById(id);
            if (student == null) { return View("NotFound"); }
            else { return View(student); }            
        }
        [HttpPost]
        public IActionResult Edit(int id, StudentDTO studentDTO)
        {
            studentService.Update(id, studentDTO);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (studentService.GetById(id) == null) { return View("NotFound"); }
            studentService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
