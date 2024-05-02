namespace EPAM_WEBAPI.Controllers
{
    using EPAM_WEBAPI.Application.DTO;
    using EPAM_WEBAPI.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Mime;

    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("v1/studyGroup")]
    public class StudyGroupController : ControllerBase
    {

        private readonly IStudyGroupService studyGroupService;

        public StudyGroupController(IStudyGroupService studyGroupService)
        {
            this.studyGroupService = studyGroupService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetStudyGroups()
        {
            var response = await this.studyGroupService.GetAllStudyGroupsAsync();

            return response != null ? Ok(response) : NotFound();
        }

        [HttpGet("GetBySubject")]
        public async Task<IActionResult> GetStudyGroupBySubjectAsync([FromQuery] string subject)
        {
            var response = await studyGroupService.SearchStudyGroupBySubjectAsync(subject);

            return response != null ? Ok(response) : NotFound();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateStudyGroup(StudyGroupDto studyGroup)
        {
            await studyGroupService.CreateStudyGroupAsync(studyGroup);

            return this.Ok();
        }

        [HttpPost("Join")]
        public async Task<IActionResult> JoinToStudyGroup([FromQuery] Guid studentId, Guid groupId)
        {
            await studyGroupService.JoinUserToStudyGroupAsync(studentId, groupId);

            return this.Ok();
        }

        [HttpPost("Remove")]
        public async Task<IActionResult> RemoveFromStudyGroup([FromQuery] Guid studentId, Guid groupId)
        {
            await studyGroupService.RemoveUserFromStudyGroupAsync(studentId, groupId);

            return this.Ok();
        }
    }
}
