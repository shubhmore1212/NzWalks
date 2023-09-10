using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    //https://localhost:portnumber/api/Students
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        //GET:
        [HttpGet]
        public IActionResult GetAllStudents()
        {
            string[] studentNames = new string[] { "shubh", "sam", "om" };
            return Ok(studentNames);
        }
    }
}
