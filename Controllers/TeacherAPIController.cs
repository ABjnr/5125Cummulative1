using _5125Cummulative1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace _5125Cummulative1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        /// <summary>
        /// This method gets all teachers from the database and outputs them in a list
        /// </summary>
        /// <returns>
        /// A list of strings representing each teacher's name
        /// </returns>
        [HttpGet("ListTeachers")]
        public List<string> ListTeachers()
        {
            List<string> teacherNames = new List<string>();

         
            // The object that represents the school db connection
            SchoolDBContext schoolContext = new SchoolDBContext();

            // (MySqlConnection) we need to connect to the db
            MySqlConnection connection = schoolContext.AccessDatabase();

            // Open up the connection to our db
            connection.Open();

            // (MySqlCommand) we need to run an SQL command
            MySqlCommand command = connection.CreateCommand();

            // Select all teachers
            command.CommandText = "SELECT * FROM teachers";
            
            MySqlDataReader resultSet = command.ExecuteReader();
            // For every one of the teachers in our result set (MySqlDataReader)
            while (resultSet.Read())
            {
                // Add the teacher's name to the list of teacher names
                string teacherfname = resultSet["teacherfname"].ToString();
                string teacherlname = resultSet["teacherlname"].ToString();

                teacherNames.Add($"{teacherfname} {teacherlname}");
            }

            // Closing the connection
            connection.Close();

            // Return the list
            return teacherNames;
        }

        /// <summary>
        /// This method gets all information on a teacher by their ID
        /// </summary>
        /// <param name="id">The ID of the teacher</param>
        /// <returns>
        /// A string representing the teacher's information
        /// </returns>
        [HttpGet("GetTeacher/{id}")]
        public string GetTeacher(int id)
        {
            string teacherInfo = string.Empty;

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
                // Get the teacher's information

                string teacherid = resultSet["teacherid"].ToString();
                string teacherfname = resultSet["teacherfname"].ToString();
                string teacherlname = resultSet["teacherlname"].ToString();
                string employeenumber = resultSet["employeenumber"].ToString();
                string hiredate = resultSet["hiredate"].ToString();
                string salary = resultSet["salary"].ToString();
                 

                teacherInfo = $"id: {teacherid}, employee number: {employeenumber}, full name: {teacherfname} {teacherlname}, hired: {hiredate}, salary: ${salary} ";
            }

            // Closing the connection
            connection.Close();

            // Return the teacher's information
            return teacherInfo;
        }
    }
}
