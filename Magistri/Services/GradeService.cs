using Magistri.DTO;
using Magistri.Models;
using Magistri.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Magistri.Services
{
    public class GradeService
    {
        private ApplicationDbContext dbContext;        
        public GradeService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<Grade> GetAllGrades()
        {
            var allGrades = dbContext.Grades.Include(x => x.Student).Include(x => x.Subject).ToList();
            return allGrades;
        }       
        public void Create(GradeDTO gradeDTO)
        { 
            dbContext.Grades.Add(DtoToModel(gradeDTO));
            dbContext.SaveChanges();
        }
        public GradesDropdownsViewModel GetNewGradesDropdownsValues()
        {
            var gradesDropdownData = new GradesDropdownsViewModel()
            {
                Students = dbContext.Students.OrderBy(x => x.LastName).ToList(),
                Subjects = dbContext.Subjects.OrderBy(x => x.Name).ToList()
            };
            return gradesDropdownData;
        }
        private Grade DtoToModel(GradeDTO gradeDTO)
        {
            return new Grade
            {
                Id = gradeDTO.Id,
                Student = dbContext.Students.FirstOrDefault(x => x.Id == gradeDTO.StudentId),
                Subject = dbContext.Subjects.FirstOrDefault(x => x.Id == gradeDTO.SubjectId),
                Title = gradeDTO.Title,
                Mark = gradeDTO.Mark,
                Date = DateOnly.FromDateTime(DateTime.Today)
            };
        }        
        public Grade GetById(int id)
        {
            var grade = dbContext.Grades.Include(n => n.Student).Include(m => m.Subject).FirstOrDefault(x => x.Id == id);
            return grade;
        }
        public void Update(int id, GradeDTO gradeDTO)
        {
            var grade = GetById(id);
            grade.Student = dbContext.Students.FirstOrDefault(n => n.Id == gradeDTO.StudentId);
            grade.Subject = dbContext.Subjects.FirstOrDefault(n => n.Id == gradeDTO.SubjectId);
            grade.Title = gradeDTO.Title;
            grade.Mark = gradeDTO.Mark;
            grade.Date = gradeDTO.Date;
            dbContext.Update(grade);
            dbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            var grade = dbContext.Grades.FirstOrDefault(x => x.Id == id);
            dbContext.Grades.Remove(grade);
            dbContext.SaveChanges();
        }
    }
}
