namespace Magistri.Models
{
    public class GradeDTO
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public string Title { get; set; }
        public int Mark { get; set; }
        public DateOnly Date { get; set; }
    }
}
