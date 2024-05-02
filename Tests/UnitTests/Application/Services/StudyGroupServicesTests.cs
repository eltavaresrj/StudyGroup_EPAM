namespace EPAM_WEBAPI.Tests.Application.Services
{
    using NUnit.Framework;
    using FluentAssertions;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EPAM_WEBAPI.Domain.Model;
    using EPAM_WEBAPI.Application.DTO;
    using EPAM_WEBAPI.Application.Services;
    using EPAM_WEBAPI.Interfaces;

    [TestFixture]
    public class StudyGroupServicesTests
    {
        private Mock<IStudyGroupRepository> mockRepository;
        private Mock<IAdaptStudyGroup> mockAdapter;
        private StudyGroupService studyGroupService;

        [SetUp]
        public void Setup()
        {
            mockRepository = new Mock<IStudyGroupRepository>();
            mockAdapter = new Mock<IAdaptStudyGroup>();
            studyGroupService = new StudyGroupService(mockRepository.Object, mockAdapter.Object);
        }

        // GetAllStudyGroupsAsync Tests

        [Test, TestCaseSource(nameof(GetAllStudyGroupsTestData))]
        public async Task GetAllStudyGroupsAsync_RepositoryReturnsStudyGroups_ReturnsListOfStudyGroupsDto(List<StudyGroup> studyGroups, List<StudyGroupResponseDto> expectedResult)
        {
            // Arrange
            mockRepository.Setup(repo => repo.GetAllStudyGroupsAsync()).ReturnsAsync(studyGroups);
            mockAdapter.Setup(adapter => adapter.AdaptStudyGroupsFromDomainToDto(studyGroups)).Returns(expectedResult);

            // Act
            var result = await studyGroupService.GetAllStudyGroupsAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task GetAllStudyGroupsAsync_RepositoryThrowsException_ReturnsException()
        {
            // Arrange
            mockRepository.Setup(repo => repo.GetAllStudyGroupsAsync()).ThrowsAsync(new Exception("Repository exception"));

            // Act & Assert
            await studyGroupService.Invoking(async s => await s.GetAllStudyGroupsAsync())
                         .Should().ThrowAsync<Exception>();
        }

        // SearchStudyGroupBySubjectAsync Tests

        [Test]
        [TestCase("Math", Subject.Math)]
        [TestCase("Chemistry", Subject.Chemistry)]
        [TestCase("Physics", Subject.Physics)]
        [TestCase("PHYSICS", Subject.Physics)]
        public async Task SearchStudyGroupBySubjectAsync_RepositoryReturnsStudyGroup_ReturnsStudyGroupDto(string subjectInput, Subject subject)
        {
            // Arrange
            var studyGroup = new StudyGroup("GroupName", subject);
            var expectedDto = new List<StudyGroupResponseDto> { new StudyGroupResponseDto("GroupName", subjectInput, new List<string>(), DateTime.Now) };

            mockRepository.Setup(repo => repo.SearchStudyGroupBySubjectAsync(subject)).ReturnsAsync(new List<StudyGroup> { studyGroup });
            mockAdapter.Setup(adapter => adapter.AdaptStudyGroupsFromDomainToDto(new List<StudyGroup> { studyGroup })).Returns(expectedDto);

            // Act
            var result = await studyGroupService.SearchStudyGroupBySubjectAsync(subjectInput);

            // Assert
            result.Should().BeEquivalentTo(expectedDto);
        }

        //// CreateStudyGroupAsync Tests

        [Test, TestCaseSource(nameof(CreateStudyGroupsTestData))]
        public async Task CreateStudyGroupAsync_AdapterReturnsStudyGroup_RepositoryReceivesStudyGroup(StudyGroupDto studyGroup, StudyGroup expectedResult)
        {
            // Arrange

            mockAdapter.Setup(adapter => adapter.AdaptStudyGroupFromDtoToDomain(studyGroup)).Returns(expectedResult);

            // Act
            await studyGroupService.CreateStudyGroupAsync(studyGroup);

            // Assert
            mockRepository.Verify(repo => repo.CreateStudyGroupAsync(expectedResult), Times.Once);
        }

        //// JoinUserToStudyGroupAsync Tests

        [Test]
        public async Task JoinUserToStudyGroupAsync_RepositoryReturnsOK_ReturnsOK()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var studyGroupId = Guid.NewGuid();

            mockRepository.Setup(repo => repo.JoinUserToStudyGroupAsync(userId, studyGroupId)).Returns(Task.CompletedTask);

            // Act & Assert
            await studyGroupService.JoinUserToStudyGroupAsync(userId, studyGroupId);
        }

        //// RemoveUserFromStudyGroupAsync Tests

        [Test]
        public async Task RemoveUserFromStudyGroupAsync_RepositoryReturnsOK_ReturnsOK()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var studyGroupId = Guid.NewGuid();

            mockRepository.Setup(repo => repo.RemoveUserFromStudyGroupAsync(userId, studyGroupId)).Returns(Task.CompletedTask);

            // Act & Assert
            await studyGroupService.RemoveUserFromStudyGroupAsync(userId, studyGroupId);
        }

        private static IEnumerable<TestCaseData> CreateStudyGroupsTestData()
        {
            yield return new TestCaseData(
                new StudyGroupDto("Group1", "Math"),
                new StudyGroup("Group1", Subject.Math))
                .SetName("Return Mapped Group");

            yield return new TestCaseData(null, null)
                .SetName("Return NULL - Create");
        }

        private static IEnumerable<TestCaseData> GetAllStudyGroupsTestData()
        {
            yield return new TestCaseData(
                new List<StudyGroup> { new StudyGroup("Group1", Subject.Math) },
                new List<StudyGroupResponseDto> { new StudyGroupResponseDto("Group1", "Math", new List<string>(), DateTime.Now) })
                .SetName("Return Mapped List");

            yield return new TestCaseData(
                new List<StudyGroup>(),
                new List<StudyGroupResponseDto>())
                .SetName("Return empty List");

            yield return new TestCaseData(null, null)
                .SetName("Return NULL - GetAll");
        }
    }

}
