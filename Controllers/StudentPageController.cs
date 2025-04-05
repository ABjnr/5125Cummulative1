using _5125Cummulative1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace _5125Cummulative1.Controllers
{
    //[Route("[controller]")]
    public class StudentPageController : Controller
    {
        private readonly StudentAPIController _api;

        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }
        /// <summary>
        /// This method gets all students from the database and displays them in a list
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
        /// <returns>
        /// A view displaying a list of students
        /// </returns>
        [HttpGet]
        [Route("[controller]/List")]
        public IActionResult List()
        {
            List<Student> students = _api.ListStudents();
            return View(students);
        }
    }
}
