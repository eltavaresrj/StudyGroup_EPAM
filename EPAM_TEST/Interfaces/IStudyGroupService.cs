namespace EPAM_WEBAPI.Interfaces
{
    using EPAM_WEBAPI.Domain.Model;
    using EPAM_WEBAPI.Application.DTO;

    public interface IStudyGroupService
    {
        public Task CreateStudyGroupAsync(StudyGroupDto studyGroup);

        public Task<List<StudyGroupResponseDto>> GetAllStudyGroupsAsync();

        public Task<List<StudyGroupResponseDto>> SearchStudyGroupBySubjectAsync(string subject);

        public Task JoinUserToStudyGroupAsync(Guid userId, Guid studyGroupId);

        public Task RemoveUserFromStudyGroupAsync(Guid userId, Guid studyGroupId);
    }
}
