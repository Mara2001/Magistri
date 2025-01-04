using Magistri.DTO;
using Magistri.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Magistri.Controllers
{
    [Authorize(Roles = "Teacher, Admin")]
    public class SubjectsController : Controller
    {
        private SubjectService subjectService;
        public SubjectsController(SubjectService subjectService)
        {
            this.subjectService = subjectService;
        }
        public IActionResult Index()
        {
            var allStudents = subjectService.GetAllSubjects();
            return View(allStudents);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(SubjectDTO subjectDTO)
        {
            if (ModelState.IsValid)
            {
                subjectService.Create(subjectDTO);
                return RedirectToAction("Index");
            }
            else { return View(); }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var subject = subjectService.GetById(id);
            if (subject == null) { return View("NotFound"); }
            else { return View(subject); }
        }
        [HttpPost]
        public IActionResult Edit(int id, SubjectDTO subjectDTO)
        {
            subjectService.Update(id, subjectDTO);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (subjectService.GetById(id) == null) { return View("NotFound"); }
            subjectService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
