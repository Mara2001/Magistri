namespace Magistri.ViewModels
{
    public class GradesViewModel
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
        public string Title { get; set; }
        public string Mark {  get; set; }
        public DateOnly Date {  get; set; }
    }
}
