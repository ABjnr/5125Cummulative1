using _5125Cummulative1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace _5125Cummulative1.Controllers
{
    [Route("[controller]")]
    public class TeacherPageController : Controller
    {
        /// <summary>
        /// This method gets all teachers from the database and displays them in a list
        /// </summary>
        /// <returns>
        /// A view displaying a list of teachers
        /// </returns>
        [HttpGet("List")]
        public IActionResult List()
        {
            List<Teacher> teachers = new List<Teacher>();

            // The object that represents the school db connection
            SchoolDBContext schoolContext = new SchoolDBContext();

            // (MySqlConnection) we need to connect to the db
            MySqlConnection connection = schoolContext.AccessDatabase();

            // Open up the connection to our db
            connection.Open();

            // (MySqlCommand) we need to run an SQL command
            MySqlCommand command = connection.CreateCommand();

            // Select all teachers
            string sql = "SELECT * FROM teachers";
            command.CommandText = sql;

            MySqlDataReader resultSet = command.ExecuteReader();
            // For every one of the teachers in our result set (MySqlDataReader)
            while (resultSet.Read())
            {
                // Create a new teacher object and add it to the list
                Teacher teacher = new Teacher
                {
                    Id = int.Parse(resultSet["teacherid"].ToString()),
                    FirstName = resultSet["teacherfname"].ToString(),
                    LastName = resultSet["teacherlname"].ToString(),
                    EmployeeNumber = resultSet["employeenumber"].ToString(),
                    HireDate = DateTime.Parse(resultSet["hiredate"].ToString()),
                    Salary = decimal.Parse(resultSet["salary"].ToString())
                };
                teachers.Add(teacher);
            }

            // Closing the connection
            connection.Close();

            // Return the list view with the teachers
            return View(teachers);
        }

        /// <summary>
        /// This method gets all information on a teacher by their ID and displays it
        /// </summary>
        /// <param name="id">The ID of the teacher</param>
        /// <returns>
        /// A view displaying the teacher's information
        /// </returns>
        [HttpGet("Show/{id}")]
        public IActionResult Show(int id)
        {
            Teacher teacher = null;

            // The object that represents the school db connection
            SchoolDBContext schoolContext = new SchoolDBContext();

            // (MySqlConnection) we need to connect to the db
            MySqlConnection connection = schoolContext.AccessDatabase();

            // Open up the connection to our db
            connection.Open();

            // (MySqlCommand) we need to run an SQL command
            MySqlCommand command = connection.CreateCommand();

            // Select teacher by ID
            string sql = "SELECT * FROM teachers WHERE teacherid = @id";
            command.CommandText = sql;
            command.Parameters.AddWithValue("@id", id);

            MySqlDataReader resultSet = command.ExecuteReader();
            // If a teacher is found
            if (resultSet.Read())
            {
                // Create a new teacher object
                teacher = new Teacher
                {
                    Id = int.Parse(resultSet["teacherid"].ToString()),
                    FirstName = resultSet["teacherfname"].ToString(),
                    LastName = resultSet["teacherlname"].ToString(),
                    EmployeeNumber = resultSet["employeenumber"].ToString(),
                    HireDate = DateTime.Parse(resultSet["hiredate"].ToString()),
                    Salary = decimal.Parse(resultSet["salary"].ToString())
                };
            }
            else
            {
                return NotFound("Teacher not found");
            }

            // Closing the connection
            connection.Close();

            // Return the show view with the teacher
            return View(teacher);
        }

    }
}
