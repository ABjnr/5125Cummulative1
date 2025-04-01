using _5125Cummulative1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace _5125Cummulative1.Controllers
{
    [Route("[controller]")]
    public class StudentPageController : Controller
    {
        /// <summary>
        /// This method gets all students from the database and displays them in a list
        /// </summary>
        /// <returns>
        /// A view displaying a list of students
        /// </returns>
        [HttpGet("List")]
        public IActionResult List()
        {
            List<Student> students = new List<Student>();

            // The object that represents the school db connection
            SchoolDBContext schoolContext = new SchoolDBContext();

            // (MySqlConnection) we need to connect to the db
            MySqlConnection connection = schoolContext.AccessDatabase();

            // Open up the connection to our db
            connection.Open();

            // (MySqlCommand) we need to run an SQL command
            MySqlCommand command = connection.CreateCommand();

            // Select all students
            string sql = "SELECT * FROM students";
            command.CommandText = sql;

            MySqlDataReader resultSet = command.ExecuteReader();
            // For every one of the students in our result set (MySqlDataReader)
            while (resultSet.Read())
            {
                // Create a new student object and add it to the list
                Student student = new Student
                {
                    Id = int.Parse(resultSet["studentid"].ToString()),
                    FirstName = resultSet["studentfname"].ToString(),
                    LastName = resultSet["studentlname"].ToString(),
                    StudentNumber = resultSet["studentnumber"].ToString(),
                    EnrollmentDate = DateTime.Parse(resultSet["enroldate"].ToString())

                };
                students.Add(student);
            }

            // Closing the connection
            connection.Close();

            // Return the list view with the students
            return View(students);
        }
    }
}
