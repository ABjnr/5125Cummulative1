using _5125Cummulative1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace _5125Cummulative1.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        /// <summary>
        /// Gets all students from the database
        /// Example return data: 
        /// [
        ///     {
        ///         "id": 1,
        ///         "firstName": "John",
        ///         "lastName": "Doe",
        ///         "studentNumber": "S12345",
        ///         "enrollmentDate": "2023-01-01T00:00:00"
        ///     }
        /// ]
        /// </summary>
        /// <returns>A list of students</returns>
        [HttpGet("api/[controller]/List")]
        public List<Student> ListStudents()
        {
            List<Student> students = new List<Student>();

            SchoolDBContext schoolContext = new SchoolDBContext();

            MySqlConnection connection = schoolContext.AccessDatabase();

            connection.Open();

            MySqlCommand command = connection.CreateCommand();

            string sql = "SELECT * FROM students";
            command.CommandText = sql;

            MySqlDataReader resultSet = command.ExecuteReader();
            while (resultSet.Read())
            {
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

            connection.Close();

            return students;

        }




        /// <summary>
        /// Adds a student to the database
        /// Example return data: 1
        /// </summary>
        /// <param name="NewStudent">The student object to add</param>
        /// <returns>The ID of the newly added student</returns>
        [HttpPost("AddStudent")]
        public IActionResult AddStudent([FromBody] Student NewStudent)
        {
            if (string.IsNullOrEmpty(NewStudent.FirstName) || string.IsNullOrEmpty(NewStudent.LastName))
            {
                return BadRequest("Student name cannot be empty.");
            }

            if (NewStudent.EnrollmentDate > DateTime.Now)
            {
                return BadRequest("Enrollment date cannot be in the future.");
            }

            using (MySqlConnection connection = new SchoolDBContext().AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                command.CommandText = "INSERT INTO students (studentfname, studentlname, studentnumber, enroldate) VALUES (@FirstName, @LastName, @StudentNumber, @EnrollmentDate)";
                command.Parameters.AddWithValue("@FirstName", NewStudent.FirstName);
                command.Parameters.AddWithValue("@LastName", NewStudent.LastName);
                command.Parameters.AddWithValue("@StudentNumber", NewStudent.StudentNumber);
                command.Parameters.AddWithValue("@EnrollmentDate", NewStudent.EnrollmentDate);

                command.ExecuteNonQuery();
                return Ok(Convert.ToInt32(command.LastInsertedId));
            }
        }



        /// <summary>
        /// Deletes a student from the database
        /// Example return data: 1
        /// </summary>
        /// <param name="id">The ID of the student to delete</param>
        /// <returns>The number of rows affected by the delete operation</returns>
        [HttpDelete("DeleteStudent/{id}")]
        public IActionResult DeleteStudent(int id)
        {
            using (MySqlConnection connection = new SchoolDBContext().AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                command.CommandText = "DELETE FROM students WHERE studentid = @id";
                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound("Student not found.");
                }

                return Ok(rowsAffected);
            }
        }



        /// <summary>
        /// Updates a student's information in the database.
        /// </summary>
        /// <param name="id">The ID of the student to update</param>
        /// <param name="UpdatedStudent">The updated student object</param>
        /// <returns>A success message or an error message</returns>
        [HttpPut("UpdateStudent/{id}")]
        public IActionResult UpdateStudent(int id, [FromBody] Student UpdatedStudent)
        {
            if (id != UpdatedStudent.Id)
            {
                return BadRequest("Student ID mismatch.");
            }

            if (string.IsNullOrEmpty(UpdatedStudent.FirstName) || string.IsNullOrEmpty(UpdatedStudent.LastName))
            {
                return BadRequest("Student name cannot be empty.");
            }

            if (UpdatedStudent.EnrollmentDate > DateTime.Now)
            {
                return BadRequest("Enrollment date cannot be in the future.");
            }

            using (MySqlConnection connection = new SchoolDBContext().AccessDatabase())
            {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();

                // Check if the student exists
                command.CommandText = "SELECT COUNT(*) FROM students WHERE studentid = @id";
                command.Parameters.AddWithValue("@id", id);
                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count == 0)
                {
                    return NotFound("Student not found.");
                }

                // Update the student's information
                command.CommandText = "UPDATE students SET studentfname = @FirstName, studentlname = @LastName, studentnumber = @StudentNumber, enroldate = @EnrollmentDate WHERE studentid = @id";
                command.Parameters.AddWithValue("@FirstName", UpdatedStudent.FirstName);
                command.Parameters.AddWithValue("@LastName", UpdatedStudent.LastName);
                command.Parameters.AddWithValue("@StudentNumber", UpdatedStudent.StudentNumber);
                command.Parameters.AddWithValue("@EnrollmentDate", UpdatedStudent.EnrollmentDate);

                command.ExecuteNonQuery();

                return Ok(new { message = "Student updated successfully." });
            }
        }



    }
}
