using _5125Cummulative1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace _5125Cummulative1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {

        private readonly SchoolDBContext _context;
        public TeacherAPIController(SchoolDBContext context)
        {
            _context = context;
        }


        /// <summary>
        /// This method gets all teachers from the database and outputs them in a list
        /// Example return data: 
        /// [
        ///     {
        ///         "id": 1,
        ///         "firstName": "Jane",
        ///         "lastName": "Smith",
        ///         "employeeNumber": "T12345",
        ///         "hireDate": "2023-01-01T00:00:00",
        ///         "salary": 50000.00
        ///     }
        /// ]
        /// </summary>
        /// <returns>
        /// A list of teachers
        /// </returns>
        [HttpGet("ListTeachers")]
        public List<Teacher> ListTeachers(string SearchKey = null)
        {
            List<Teacher> AllTeachers = new List<Teacher>();


            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                string query = "SELECT * FROM teachers";

                if (SearchKey != null)
                {
                    query += " WHERE LOWER(teacherfname) LIKE @key OR LOWER(teacherlname) LIKE @key OR LOWER(CONCAT(teacherfname,' ',teacherlname)) LIKE @key";
                    Command.Parameters.AddWithValue("@key", $"%{SearchKey.ToLower()}%");
                }
                Command.CommandText = query;
                Command.Prepare();

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        Teacher CurrentTeacher = new Teacher()
                        {
                            Id = Convert.ToInt32(ResultSet["teacherid"]),
                            FirstName = ResultSet["teacherfname"].ToString(),
                            LastName = ResultSet["teacherlname"].ToString(),
                            EmployeeNumber = ResultSet["employeenumber"].ToString(),
                            HireDate = Convert.ToDateTime(ResultSet["hiredate"]),
                            Salary = Convert.ToDecimal(ResultSet["salary"])

                        };
                        AllTeachers.Add(CurrentTeacher);
                    }
                }
            }
            return AllTeachers;
        }


        /// <summary>
        /// This method gets all information on a teacher by their ID
        /// Example return data: 
        /// {
        ///     "id": 1,
        ///     "firstName": "Jane",
        ///     "lastName": "Smith",
        ///     "employeeNumber": "T12345",
        ///     "hireDate": "2023-01-01T00:00:00",
        ///     "salary": 50000.00
        /// }
        /// </summary>
        /// <param name="id">The ID of the teacher</param>
        /// <returns>
        /// The teacher object
        /// </returns>
        [HttpGet("GetTeacher/{id}")]
        public Teacher GetTeacher(int id)
        {
            Teacher teacherInfo = new Teacher();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "SELECT * FROM teachers WHERE teacherid = @id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    if (ResultSet.Read())
                    {
                        teacherInfo = new Teacher()
                        {
                            Id = Convert.ToInt32(ResultSet["teacherid"]),
                            FirstName = ResultSet["teacherfname"].ToString(),
                            LastName = ResultSet["teacherlname"].ToString(),
                            EmployeeNumber = ResultSet["employeenumber"].ToString(),
                            HireDate = Convert.ToDateTime(ResultSet["hiredate"]),
                            Salary = Convert.ToDecimal(ResultSet["salary"])
                        };
                    }
                }
            }
            return teacherInfo;
        }

        /// <summary>
        /// Adds a teacher to the database
        /// Example return data: 1
        /// </summary>
        /// <param name="NewTeacher">The teacher object to add</param>
        /// <returns>
        /// The ID of the newly added teacher
        /// </returns>
        [HttpPost("AddTeacher")]
        public IActionResult AddTeacher([FromBody] Teacher NewTeacher)
        {
            if (string.IsNullOrEmpty(NewTeacher.FirstName) || string.IsNullOrEmpty(NewTeacher.LastName))
            {
                return BadRequest("Teacher name cannot be empty.");
            }

            if (NewTeacher.HireDate > DateTime.Now)
            {
                return BadRequest("Hire date cannot be in the future.");
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(NewTeacher.EmployeeNumber, @"^T\d+$"))
            {
                return BadRequest("Employee number must start with 'T' followed by digits.");
            }

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "SELECT COUNT(*) FROM teachers WHERE employeenumber = @EmployeeNumber";
                Command.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
                int count = Convert.ToInt32(Command.ExecuteScalar());

                if (count > 0)
                {
                    return BadRequest("Employee number is already taken by a different teacher.");
                }

                Command.CommandText = "INSERT INTO teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) VALUES (@FirstName, @LastName, @EmployeeNumber, @HireDate, @Salary)";
                Command.Parameters.AddWithValue("@FirstName", NewTeacher.FirstName);
                Command.Parameters.AddWithValue("@LastName", NewTeacher.LastName);
                Command.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
                Command.Parameters.AddWithValue("@Salary", NewTeacher.Salary);

                Command.ExecuteNonQuery();
                return Ok(Convert.ToInt32(Command.LastInsertedId));
            }
        }

        /// <summary>
        /// Deletes a teacher from the database
        /// Example return data: 1
        /// </summary>
        /// <param name="id">The ID of the teacher to delete</param>
        /// <returns>
        /// The number of rows affected by the delete operation
        /// </returns>
        [HttpDelete("DeleteTeacher/{id}")]
        public IActionResult DeleteTeacher(int id)
        {
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "DELETE FROM teachers WHERE teacherid = @id";
                Command.Parameters.AddWithValue("@id", id);

                int rowsAffected = Command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    return NotFound("Teacher not found.");
                }
                return Ok(new { rowsAffected, redirectUrl = Url.Action("List", "TeacherPage") });
            }
        }



        /// <summary>
        /// Updates a teacher's information in the database.
        /// Example request body:
        /// {
        ///     "id": 1,
        ///     "firstName": "UpdatedFirstName",
        ///     "lastName": "UpdatedLastName",
        ///     "employeeNumber": "T12345",
        ///     "hireDate": "2023-01-01T00:00:00",
        ///     "salary": 60000.00
        /// }
        /// </summary>
        /// <param name="id">The ID of the teacher to update</param>
        /// <param name="UpdatedTeacher">The updated teacher object</param>
        /// <returns>
        /// A success message or an error message
        /// </returns>




        [HttpPut("UpdateTeacher/{id}")]
        public IActionResult UpdateTeacher(int id, [FromBody] Teacher UpdatedTeacher)
        {
            if (id != UpdatedTeacher.Id)
            {
                return BadRequest("Teacher ID mismatch.");
            }

            if (string.IsNullOrEmpty(UpdatedTeacher.FirstName) || string.IsNullOrEmpty(UpdatedTeacher.LastName))
            {
                return BadRequest("Teacher name cannot be empty.");
            }

            if (UpdatedTeacher.HireDate > DateTime.Now)
            {
                return BadRequest("Hire date cannot be in the future.");
            }

            if (UpdatedTeacher.Salary < 0)
            {
                return BadRequest("Salary cannot be less than 0.");
            }

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "SELECT COUNT(*) FROM teachers WHERE teacherid = @id";
                Command.Parameters.AddWithValue("@id", id);
                int count = Convert.ToInt32(Command.ExecuteScalar());

                if (count == 0)
                {
                    return NotFound("Teacher not found.");
                }

                Command.CommandText = "UPDATE teachers SET teacherfname = @FirstName, teacherlname = @LastName, employeenumber = @EmployeeNumber, hiredate = @HireDate, salary = @Salary WHERE teacherid = @id";
                Command.Parameters.AddWithValue("@FirstName", UpdatedTeacher.FirstName);
                Command.Parameters.AddWithValue("@LastName", UpdatedTeacher.LastName);
                Command.Parameters.AddWithValue("@EmployeeNumber", UpdatedTeacher.EmployeeNumber);
                Command.Parameters.AddWithValue("@HireDate", UpdatedTeacher.HireDate);
                Command.Parameters.AddWithValue("@Salary", UpdatedTeacher.Salary);

                Command.ExecuteNonQuery();

                // Return success with a hardcoded redirect URL
                return Ok(new { message = "Teacher updated successfully.", redirectUrl = "/TeacherPage/List" });
            }
        }




    }
}
