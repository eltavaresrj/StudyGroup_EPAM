namespace EPAM_WEBAPI.Application.Adapters
{
    using EPAM_WEBAPI.Domain.Model;
    using EPAM_WEBAPI.Application.DTO;
    using EPAM_WEBAPI.Interfaces;

    public class AdaptStudyGroup : IAdaptStudyGroup
    {
        private Subject subject;

        public StudyGroup AdaptStudyGroupFromDtoToDomain(StudyGroupDto studyGroupDto)
        {
            if (studyGroupDto == null) 
            {
                return null;
            }

            if (!Enum.TryParse(studyGroupDto.Subject, true, out Subject subject))
            {
                throw new ArgumentException($"{studyGroupDto.Subject} is not a valid subject!");
            }

            return new StudyGroup(studyGroupDto.Name, subject);
        }

        public List<StudyGroupResponseDto> AdaptStudyGroupsFromDomainToDto(List<StudyGroup> studyGroups)
        {
            if (studyGroups == null)
            {
                return null;
            }

            var studyGroupsAdapted = new List<StudyGroupResponseDto>();

            foreach (var sg in studyGroups)
            {
                var studentsNames = sg.Student?.Select(x => x.Name).ToList();
                var subject = sg.Subject.ToString();

                var studyGroup = new StudyGroupResponseDto(sg.Name, subject, studentsNames, sg.CreatedDate);

                studyGroupsAdapted.Add(studyGroup);

            }

            return studyGroupsAdapted;
        }
    }
}
