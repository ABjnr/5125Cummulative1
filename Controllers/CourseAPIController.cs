using _5125Cummulative1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace _5125Cummulative1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseAPIController : ControllerBase
    {
        /// <summary>
        /// Gets all courses from the database
        /// Example return data: 
        /// [
        ///     {
        ///         "id": 1,
        ///         "courseCode": "CS101",
        ///         "teacherId": 2,
        ///         "startDate": "2023-01-01T00:00:00",
        ///         "finishDate": "2023-06-01T00:00:00",
        ///         "courseName": "Introduction to Computer Science"
        ///     }
        /// ]
        /// </summary>
        /// <returns>A list of courses</returns>
        [HttpGet("List")]
        public List<Course> ListCourses()
        {
            List<Course> courses = new List<Course>();

            SchoolDBContext schoolContext = new SchoolDBContext();

            MySqlConnection connection = schoolContext.AccessDatabase();

            connection.Open();

            MySqlCommand command = connection.CreateCommand();

            string sql = "SELECT * FROM courses";
            command.CommandText = sql;

            MySqlDataReader resultSet = command.ExecuteReader();
            while (resultSet.Read())
            {
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

            connection.Close();

            return courses;
        }


        /// <summary>
        /// Adds a course to the database
        /// Example return data: 1
        /// </summary>
        /// <param name="NewCourse">The course object to add</param>
        /// <returns>The ID of the newly added course</returns>
        [HttpPost("AddCourse")]
        public IActionResult AddCourse([FromBody] Course NewCourse)
        {
            if (NewCourse.Id <= 0)
            {
                return BadRequest("Course ID must be a positive integer.");
            }

            if (string.IsNullOrEmpty(NewCourse.CourseName) || string.IsNullOrEmpty(NewCourse.CourseCode))
            {
                return BadRequest("Course name and code cannot be empty.");
            }

            if (NewCourse.StartDate > NewCourse.FinishDate)
            {
                return BadRequest("Start date cannot be after finish date.");
            }

            using (MySqlConnection connection = new SchoolDBContext().AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                command.CommandText = "INSERT INTO courses (courseid, coursecode, teacherid, startdate, finishdate, coursename) VALUES (@CourseId, @CourseCode, @TeacherId, @StartDate, @FinishDate, @CourseName)";
                command.Parameters.AddWithValue("@CourseId", NewCourse.Id);
                command.Parameters.AddWithValue("@CourseCode", NewCourse.CourseCode);
                command.Parameters.AddWithValue("@TeacherId", NewCourse.TeacherId);
                command.Parameters.AddWithValue("@StartDate", NewCourse.StartDate);
                command.Parameters.AddWithValue("@FinishDate", NewCourse.FinishDate);
                command.Parameters.AddWithValue("@CourseName", NewCourse.CourseName);

                command.ExecuteNonQuery();
                return Ok(NewCourse.Id);
            }
        }


        /// <summary>
        /// Deletes a course from the database
        /// Example return data: 1
        /// </summary>
        /// <param name="id">The ID of the course to delete</param>
        /// <returns>The number of rows affected by the delete operation</returns>
        [HttpDelete("DeleteCourse/{id}")]
        public IActionResult DeleteCourse(int id)
        {
            using (MySqlConnection connection = new SchoolDBContext().AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                command.CommandText = "DELETE FROM courses WHERE courseid = @id";
                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound("Course not found.");
                }

                return Ok(rowsAffected);
            }  
        }





        /// <summary>
        /// Updates a course's information in the database.
        /// </summary>
        /// <param name="id">The ID of the course to update</param>
        /// <param name="UpdatedCourse">The updated course object</param>
        /// <returns>A success message or an error message</returns>
        [HttpPut("UpdateCourse/{id}")]
        public IActionResult UpdateCourse(int id, [FromBody] Course UpdatedCourse)
        {
            if (id != UpdatedCourse.Id)
            {
                return BadRequest("Course ID mismatch.");
            }

            if (string.IsNullOrEmpty(UpdatedCourse.CourseCode) || string.IsNullOrEmpty(UpdatedCourse.CourseName))
            {
                return BadRequest("Course code and name cannot be empty.");
            }

            if (UpdatedCourse.StartDate > UpdatedCourse.FinishDate)
            {
                return BadRequest("Start date cannot be after the finish date.");
            }

            using (MySqlConnection connection = new SchoolDBContext().AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                // Check if the course exists
                command.CommandText = "SELECT COUNT(*) FROM courses WHERE courseid = @id";
                command.Parameters.AddWithValue("@id", id);
                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count == 0)
                {
                    return NotFound("Course not found.");
                }

                // Update the course's information
                command.CommandText = @"
            UPDATE courses 
            SET 
                coursecode = @CourseCode, 
                coursename = @CourseName, 
                teacherid = @TeacherId, 
                startdate = @StartDate, 
                finishdate = @FinishDate 
            WHERE courseid = @id";
                command.Parameters.AddWithValue("@CourseCode", UpdatedCourse.CourseCode);
                command.Parameters.AddWithValue("@CourseName", UpdatedCourse.CourseName);
                command.Parameters.AddWithValue("@TeacherId", UpdatedCourse.TeacherId);
                command.Parameters.AddWithValue("@StartDate", UpdatedCourse.StartDate);
                command.Parameters.AddWithValue("@FinishDate", UpdatedCourse.FinishDate);

                command.ExecuteNonQuery();

                return Ok(new { message = "Course updated successfully." });
            }
        }



    }
}
