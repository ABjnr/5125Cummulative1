using _5125Cummulative1.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace _5125Cummulative1.Controllers
{
    //[Route("[controller]")]
    public class TeacherPageController : Controller
    {
        private readonly TeacherAPIController _api;

        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }

        /// <summary>
        /// This method gets all teachers from the database and displays them in a list
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
        /// A view displaying a list of teachers
        /// </returns>

        [HttpGet]
        [Route("[controller]/List")]
        public IActionResult List(string SearchKey = null)
        {
            List<Teacher> teachers = _api.ListTeachers(SearchKey);
            return View(teachers);
        }

        /// <summary>
        /// This method gets all information on a teacher by their ID and displays it
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
        /// A view displaying the teacher's information
        /// </returns>
        [HttpGet]
        [Route("[controller]/Show/{id}")]
        public IActionResult Show(int id)
        {
            Teacher teacher = _api.GetTeacher(id);
            if (teacher.Id == 0)
            {
                return View("NotFound");
            }
            return View(teacher);

        }



        /// <summary>
        /// This method displays a form to add a new teacher
        /// </summary>
        /// <returns>
        /// A view displaying the form to add a new teacher
        /// </returns>
        [HttpGet]
        [Route("[controller]/New")]
        public IActionResult New()
        {
            return View();
        }



        /// <summary>
        /// This method adds a new teacher to the database
        /// Example return data: 1
        /// </summary>
        /// <param name="NewTeacher">The teacher object to add</param>
        /// <returns>
        /// Redirects to the Show view of the newly added teacher
        /// </returns>
        [HttpPost]
        [Route("[controller]/Create")]
        public IActionResult Create(Teacher NewTeacher)
        {
            IActionResult result = _api.AddTeacher(NewTeacher);
            if (result is CreatedAtActionResult createdResult)
            {
                int TeacherId = (int)createdResult.RouteValues["id"];
                return RedirectToAction("Show", new { id = TeacherId });
            }
            return result;
        }



        /// <summary>
        /// This method displays a confirmation page to delete a teacher
        /// </summary>
        /// <param name="id">The ID of the teacher to delete</param>
        /// <returns>
        /// A view displaying the confirmation page to delete a teacher
        /// </returns>
        [HttpGet]
        [Route("[controller]/DeleteConfirm/{id}")]
        public IActionResult DeleteConfirm(int id)
        {
           
            Teacher teacher = _api.GetTeacher(id);
            if (teacher == null)
            {
               
                return View("NotFound");
            }
            return View(teacher);
        }



        /// <summary>
        /// This method deletes a teacher from the database
        /// Example return data: 1
        /// </summary>
        /// <param name="id">The ID of the teacher to delete</param>
        /// <returns>
        /// Redirects to the List view after deleting the teacher
        /// </returns>
        [HttpPost]
        [Route("[controller]/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            IActionResult result = _api.DeleteTeacher(id);
            if (result is OkResult)
            {
                return RedirectToAction("List");
            }
            return result;
        }

    }
}
