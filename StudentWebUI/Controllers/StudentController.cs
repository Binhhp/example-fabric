
using GroupStudent.Utils;
using GroupStudent.Interfaces;
using Microsoft.AspNetCore.Mvc;
using StudentDomain;
using StudentDomain.Constants;
using System;
using System.Net.Mime;
using System.Threading.Tasks;

namespace StudentWebUI.Controllers
{
    [ApiController]
    [Route("api/student")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class StudentController : ControllerBase
    {
        private readonly IGroupStudent actor;
        public StudentController()
        {
            actor = (new StudentActorCreate(ActorIdConstant.DefaultID, FabricUriConstants.UriGroupStudentActor)).GetGroupStudent();
        }
        [HttpGet]
        public async Task<IActionResult> GetStudent([FromQuery]string id, [FromQuery]string name)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    var students = await actor.GetListStudentAsync();
                    return Ok(students);
                }
                var studentModel = await actor.GetCurrentStudentAsync(new Guid(id));
                return Ok(studentModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent([FromBody]Student student)
        {
            try
            {
                var result = await actor.AddOrUpdateStudentAsync(student);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{studentId}")]
        public IActionResult DeleteStudent([FromQuery]string studentId)
        {
            try
            {
                actor.DeleteStudentAsync(new Guid(studentId));
                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
