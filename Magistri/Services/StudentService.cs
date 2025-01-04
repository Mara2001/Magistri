using Magistri.Models;
using Microsoft.EntityFrameworkCore;

namespace Magistri.Services
{
    public class StudentService
    {
        private ApplicationDbContext dbContext;

        public StudentService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //internal IEnumerable<StudentDto> GetAllStudents()
        //{
        //    var allStudents = dbContext.Students;
        //    var studentsDtos = new List<StudentDto>();
        //    foreach (var student in allStudents)
        //    {
        //        studentsDtos.Add(new StudentDto
        //        {
        //            Id = student.Id,
        //            DateOfBirth = student.DateOfBirth,
        //            FirstName = student.FirstName,
        //            LastName = student.LastName
        //        });
        //    }
        //    return studentsDtos;
        //}
        public IEnumerable<StudentDTO> GetAllStudents()
        {
            var allStudents = dbContext.Students;
            var students = new List<StudentDTO>();
            foreach (var student in allStudents)
            {
                students.Add(new StudentDTO
                {
                    Id = student.Id,
                    DateOfBirth = student.DateOfBirth,
                    FirstName = student.FirstName,
                    LastName = student.LastName
                });
            }
            return students;
        }
        public void Create(StudentDTO studentDTO)
        {
            dbContext.Students.Add(DtoToModel(studentDTO));
            dbContext.SaveChanges();
        }
        public StudentDTO Update(int id, StudentDTO studentDTO)
        {        
            dbContext.Update(DtoToModel(studentDTO));
            dbContext.SaveChanges();
            return studentDTO;
        }
        public void Delete(int id)
        { 
            var student = dbContext.Students.FirstOrDefault(x => x.Id == id);
            dbContext.Students.Remove(student);
            dbContext.SaveChanges();
        }
        private StudentDTO ModelToDto(Student student)
        {
            return new StudentDTO
            {
                Id = student.Id,
                DateOfBirth = student.DateOfBirth,
                FirstName = student.FirstName,
                LastName = student.LastName
            };
        }
        public Student DtoToModel(StudentDTO studentDTO)
        { 
            return new Student {
                Id = studentDTO.Id,
                DateOfBirth = studentDTO.DateOfBirth,
                FirstName=studentDTO.FirstName,
                LastName = studentDTO.LastName};
        }
        public StudentDTO GetById(int id)
        {
            var student = dbContext.Students.FirstOrDefault(x => x.Id == id);
            return ModelToDto(student);
        }

    }
}
