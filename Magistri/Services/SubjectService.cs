//using Magistri.Migrations;
using Magistri.Models;

namespace Magistri.Services
{
    public class SubjectService
    {
        private ApplicationDbContext dbContext;
        public SubjectService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<SubjectDTO> GetAllSubjects()
        {
            var allSubjects = dbContext.Subjects;
            var subjects = new List<SubjectDTO>();
            foreach (var subject in allSubjects)
            {
                subjects.Add(new SubjectDTO
                {
                    Id = subject.Id,
                    Name = subject.Name
                });
            }
            return subjects;
        }
        public void Create(SubjectDTO subjectDTO)
        {
            dbContext.Subjects.Add(DtoToModel(subjectDTO));
            dbContext.SaveChanges();
        }
        public SubjectDTO Update(int id, SubjectDTO subjectDTO)
        {
            dbContext.Update(DtoToModel(subjectDTO));
            dbContext.SaveChanges();
            return subjectDTO;
        }
        public void Delete(int id)
        {
            var subject = dbContext.Subjects.FirstOrDefault(x => x.Id == id);
            dbContext.Subjects.Remove(subject);
            dbContext.SaveChanges();
        }
        public Subject DtoToModel(SubjectDTO subjectDTO)
        {
            return new Subject
            {
                Id = subjectDTO.Id,
                Name = subjectDTO.Name
            };
        }
        public SubjectDTO GetById(int id)
        {
            var subject = dbContext.Subjects.FirstOrDefault(x => x.Id == id);
            return ModelToDto(subject);
        }
        private SubjectDTO ModelToDto(Subject subject)
        {
            return new SubjectDTO
            {
                Id = subject.Id,
                Name = subject.Name
            };
        }
    }
}
