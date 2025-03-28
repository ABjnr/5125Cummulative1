namespace _5125Cummulative1.Models
{
    /// <summary>
    /// Represents a teacher in the school database.
    /// </summary>
    public class Teacher
    {
        /// <summary>
        /// Gets or sets the ID of the teacher.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the first name of the teacher.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the teacher.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the employee number of the teacher.
        /// </summary>
        public string EmployeeNumber { get; set; }

        /// <summary>
        /// Gets or sets the hire date of the teacher.
        /// </summary>
        public DateTime HireDate { get; set; }

        /// <summary>
        /// Gets or sets the salary of the teacher.
        /// </summary>
        public decimal Salary { get; set; }
    }
}
