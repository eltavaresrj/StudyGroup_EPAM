namespace EPAM_WEBAPI.Interfaces
{
    using EPAM_WEBAPI.Domain.Model;
    using EPAM_WEBAPI.Application.DTO;

    public interface IAdaptStudyGroup
    {
        public StudyGroup AdaptStudyGroupFromDtoToDomain(StudyGroupDto studyGroupDto);

        public List<StudyGroupResponseDto> AdaptStudyGroupsFromDomainToDto(List<StudyGroup> studyGroups);
    }
}
