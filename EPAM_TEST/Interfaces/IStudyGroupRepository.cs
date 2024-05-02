namespace EPAM_WEBAPI.Interfaces
{
    using EPAM_WEBAPI.Domain.Model;

    public interface IStudyGroupRepository
    {
        public Task CreateStudyGroupAsync(StudyGroup studyGroup);

        public Task<List<StudyGroup>> GetAllStudyGroupsAsync();

        public Task<List<StudyGroup>> SearchStudyGroupBySubjectAsync(Subject subject);

        public Task JoinUserToStudyGroupAsync(Guid userId, Guid studyGroupId);

        public Task RemoveUserFromStudyGroupAsync(Guid userId, Guid studyGroupId);
    }
}
