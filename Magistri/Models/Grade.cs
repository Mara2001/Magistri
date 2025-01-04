namespace Magistri.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public Student Student { get; set; }
        public Subject Subject { get; set; }
        public string Title { get; set; }
        public int Mark { get; set; }
        public DateOnly Date { get; set; }
    }
}
