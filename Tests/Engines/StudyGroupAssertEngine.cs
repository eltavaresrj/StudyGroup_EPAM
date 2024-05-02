namespace EPAM_WEBAPI.Tests.Engines
{
    using EPAM_WEBAPI.Domain.Model;
    using EPAM_WEBAPI.Application.DTO;
    using EPAM_WEBAPI.Configurations.DbContext;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class StudyGroupAssertEngine
    {
        private ApplicationDbContext applicationDbContext;

        public StudyGroupAssertEngine(ApplicationDbContext applicationDbContext) 
        {
            this.applicationDbContext = applicationDbContext;
        }

        public void ValidateStudyGroups(List<StudyGroupResponseDto> actualResponse, List<StudyGroupResponseDto> expectedResponse) 
        {
            actualResponse.Should().HaveCount(expectedResponse.Count());

            foreach (var studyGroup in actualResponse) 
            {
                var expected = expectedResponse.SingleOrDefault(x => x.Subject == studyGroup.Subject);

                studyGroup.Students.Should().BeEquivalentTo(expected.Students);
                studyGroup.Name.Should().BeEquivalentTo(expected.Name);
            }
        }

        public async void ValidateStudyGroup(StudyGroup expected)
        {
            var studyGroup = await applicationDbContext.StudyGroup.FirstOrDefaultAsync();

            studyGroup.Name.Should().Be(expected.Name);
            studyGroup.Subject.Should().Be(expected.Subject);
            studyGroup.Student.Should().BeEquivalentTo(expected.Student);
        }

        public async void ValidateStudyGroupJoined(StudyGroup expected)
        {
            var studyGroup = await applicationDbContext.StudyGroup.SingleOrDefaultAsync(s => s.Name == expected.Name);

            studyGroup.Subject.Should().Be(expected.Subject);
            studyGroup.Student.Should().BeEquivalentTo(expected.Student);
        }

        public async void ValidateStudyGroupRemoved(Guid studyGroupId, Guid studentId)
        {
            var studyGroup = await applicationDbContext.StudyGroup.SingleOrDefaultAsync(s => s.Id == studyGroupId);

            studyGroup.Student.Select(x => x.Id).Should().NotContain(studentId);
        }
    }
}
