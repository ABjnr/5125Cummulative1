namespace _5125Cummulative1.Models
{
    /// <summary>
    /// Represents the details of a teacher in the school database
    /// </summary>
    public class Teacher
    {        
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
    }
}
