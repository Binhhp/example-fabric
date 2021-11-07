using GroupStudent.Utils;
using Microsoft.AspNetCore.Mvc;
using StudentDomain.Constants;
using StudentDomain.Entities;
using StudentDomain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentWebUI.Controllers
{
    [ApiController]
    [Route("api/subject")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subject;
        public SubjectController()
        {
            _subject = (new StudentActorCreate("",
                FabricUriConstants.UriGroupStudentActor, true))
                .GetSubjectStateful();
        }
        [HttpGet]
        public async Task<ActionResult<List<Subject>>> GetSubjects()
        {
            var subjects = (await _subject.GetSubjectsAsync()).ToList();
            return Ok(subjects);
        }
        [HttpPost]
        public async Task<IActionResult> PostSubjects([FromBody]Subject subject)
        {
            await _subject.AddSubjectAsync(subject);
            return Ok("Add subject success");
        }
    }
}
