using _5125Cummulative1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace _5125Cummulative1.Controllers
{
    [Route("[controller]")]
    public class CoursePageController : Controller
    {
        [HttpGet("List")]
        public IActionResult List()
        {
            List<Course> courses = new List<Course>();

            // The object that represents the school db connection
            SchoolDBContext schoolContext = new SchoolDBContext();

            // (MySqlConnection) we need to connect to the db
            MySqlConnection connection = schoolContext.AccessDatabase();

            // Open up the connection to our db
            connection.Open();

            // (MySqlCommand) we need to run an SQL command
            MySqlCommand command = connection.CreateCommand();

            // Select all courses
            string sql = "SELECT * FROM courses";
            command.CommandText = sql;

            MySqlDataReader resultSet = command.ExecuteReader();
            // For every one of the courses in our result set (MySqlDataReader)
            while (resultSet.Read())
            {
                // Create a new course object and add it to the list
                Course course = new Course
                {
                    Id = int.Parse(resultSet["courseid"].ToString()),
                    CourseCode = resultSet["coursecode"].ToString(),
                    TeacherId = int.Parse(resultSet["teacherid"].ToString()),
                    StartDate = DateTime.Parse(resultSet["startdate"].ToString()),
                    FinishDate = DateTime.Parse(resultSet["finishdate"].ToString()),
                    CourseName = resultSet["coursename"].ToString()
                };
                courses.Add(course);
            }

            // Closing the connection
            connection.Close();

            // Return the list view with the courses
            return View(courses);
        }
    }
}
