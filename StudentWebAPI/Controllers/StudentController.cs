using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StudentData.IStudent;
using StudentData.Model;

namespace StudentWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentData _studentService;
        public StudentController(ILogger<StudentController> logger, IStudentData studentDataService)
        {
            _logger = logger;
            _studentService = studentDataService;
        }

        //api/student
        [HttpGet]
        public IActionResult Index()
        {
            Models.Response response = new Models.Response();
            response.ResponseCode = "201";
            response.ResponseMessage = "Student Data Api";

            return Ok(new JsonResult(response));
        }

        //GET: api/student/{ID}
        [HttpGet("{id}")]
        public IActionResult GetStudentByID(int id)
        {
            Models.Response response = new Models.Response();

            if (id <= 0)
            {
                response.ResponseCode = "400";
                response.ResponseMessage = "Invalid Student Id Entry";
                return NotFound(new JsonResult(response));
            }

            Student student = _studentService.GetStudentByID(id, out string message);
            if (student == null)
            {
                response.ResponseCode = "400";
                response.ResponseMessage = message;
                return NotFound(new JsonResult(response));
            }

            response.ResponseCode = "201";
            response.ResponseMessage = "Student Record Retrieved Successfully";
            response.ResponseObject = student;
            return Ok(new JsonResult(response));
        }

        //POST: api/student
        [HttpPost]
        public IActionResult AddStudent([FromBody] Student student)
        {
            Models.Response response = new Models.Response();

            bool IsAdded = _studentService.AddStudent(student.Name, student.FamilyName, student.Address, student.CountryOfOrigin, student.EmailAddress, student.Age, student.Approved = false, out string message);
            if (!IsAdded)
            {
                response.ResponseCode = "400";
                response.ResponseMessage = message;
                return NotFound(new JsonResult(response));
            }

            response.ResponseCode = "201";
            response.ResponseMessage = "Student Created Successfully.";
            response.ResponseObject = student;
            return Ok(new JsonResult(response));
        }

        //PUT: api/student/{ID}
        [HttpPut("{id}")]
        public IActionResult UpdateStudentInfo([FromBody] Student student, int id)
        {
            Models.Response response = new Models.Response();

            bool IsUpdated = _studentService.UpdateStudent(id, student, out string message);
            if (!IsUpdated)
            {
                response.ResponseCode = "400";
                response.ResponseMessage = message;
                return NotFound(new JsonResult(response));
            }

            response.ResponseCode = "201";
            response.ResponseMessage = "Student Record Updated Successfully";
            return Ok(new JsonResult(response));
        }

        //DELETE: api/student/{ID}
        [HttpDelete("{id}")]
        public IActionResult DeleteStudentByID(int id)
        {
            Models.Response response = new Models.Response();

            if (id <= 0)
            {
                response.ResponseCode = "400";
                response.ResponseMessage = "Invalid Student Id Entry";
                return NotFound(new JsonResult(response));
            }

            bool IsRemoved = _studentService.DeleteStudent(id, out string message);
            if (!IsRemoved)
            {
                response.ResponseCode = "400";
                response.ResponseMessage = message;
                return NotFound(new JsonResult(response));
            }

            response.ResponseCode = "201";
            response.ResponseMessage = "Student Deleted Successfully";
            return Ok(new JsonResult(response));
        }

       
    }
}
