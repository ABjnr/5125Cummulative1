using _5125Cummulative1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace _5125Cummulative1.Controllers
{

    [Route("[controller]")]
    public class CoursePageController : Controller
    {
        private readonly CourseAPIController _api;

        public CoursePageController(CourseAPIController api)
        {
            _api = api;
        }

        /// <summary>
        /// This method gets all courses from the database and displays them in a list
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
        /// <returns>
        /// A view displaying a list of courses
        /// </returns>
        [HttpGet]
        [Route("[controller]/List")]
        public IActionResult List()
        {
            List<Course> courses = _api.ListCourses();
            return View(courses);
        }


    }
}
