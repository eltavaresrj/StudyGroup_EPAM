namespace EPAM_WEBAPI.Application.Services
{
    using EPAM_WEBAPI.Domain.Model;
    using EPAM_WEBAPI.Application.DTO;
    using EPAM_WEBAPI.Interfaces;
    using System;
    using System.Collections.Generic;

    public class StudyGroupService : IStudyGroupService
    {
        private IStudyGroupRepository studyGroupRepository;
        private IAdaptStudyGroup adaptStudyGroup;

        public StudyGroupService(IStudyGroupRepository studyGroupRepository, IAdaptStudyGroup adaptStudyGroup)
        {
            this.studyGroupRepository = studyGroupRepository;
            this.adaptStudyGroup = adaptStudyGroup;
        }

        public async Task CreateStudyGroupAsync(StudyGroupDto studyGroupDto)
        {
            var studyGroup = this.adaptStudyGroup.AdaptStudyGroupFromDtoToDomain(studyGroupDto);

            await studyGroupRepository.CreateStudyGroupAsync(studyGroup);
        }

        public async Task<List<StudyGroupResponseDto>> GetAllStudyGroupsAsync()
        {
            var studyGroups = await studyGroupRepository.GetAllStudyGroupsAsync();

            return this.adaptStudyGroup.AdaptStudyGroupsFromDomainToDto(studyGroups);
        }

        public async Task JoinUserToStudyGroupAsync(Guid userId, Guid studyGroupId)
        {
            await this.studyGroupRepository.JoinUserToStudyGroupAsync(userId, studyGroupId);
        }

        public async Task RemoveUserFromStudyGroupAsync(Guid userId, Guid studyGroupId)
        {
            await this.studyGroupRepository.RemoveUserFromStudyGroupAsync(userId, studyGroupId);
        }


        public async Task<List<StudyGroupResponseDto>> SearchStudyGroupBySubjectAsync(string subject)
        {
            if (!Enum.TryParse(subject, true, out Subject convertedSubject))
                throw new ArgumentException($"{subject} is not a valid subject!");

            var studyGroups =  await this.studyGroupRepository.SearchStudyGroupBySubjectAsync(convertedSubject);

            return this.adaptStudyGroup.AdaptStudyGroupsFromDomainToDto(studyGroups);
        }
    }
}
