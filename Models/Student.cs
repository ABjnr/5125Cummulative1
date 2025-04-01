﻿namespace _5125Cummulative1.Models
{
    /// <summary>
    /// Represents the details of a teacher in the school database
    /// </summary>
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentNumber { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
