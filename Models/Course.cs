namespace _5125Cummulative1.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string CourseCode { get; set; }
        public int TeacherId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string CourseName { get; set; }
    }
}
