namespace EPAM_WEBAPI.Tests.Controllers
{
    using EPAM_WEBAPI.Application.DTO;
    using EPAM_WEBAPI.Controllers;
    using EPAM_WEBAPI.Interfaces;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Moq;

    [TestFixture]
    public class StudyGroupControllerTests
    {
        private Mock<IStudyGroupService> studyGroupServiceMock;
        private StudyGroupController studyGroupController;

        private List<string> students;
        private List<StudyGroupResponseDto> studyGroups;

        [SetUp]
        public void Setup() 
        {
            this.studyGroupServiceMock = new Mock<IStudyGroupService>();
            this.studyGroupController = new StudyGroupController(this.studyGroupServiceMock.Object);

            this.students = new List<string>() { "Gabriel", "Tavares" };

            this.studyGroups = new List<StudyGroupResponseDto>
            {
                new StudyGroupResponseDto("Name", "Math", students, DateTime.Now)
            };
        }

        [Test]
        public async Task GetStudyGroups_Returns_OkResult_With_StudyGroups()
        {
            // Arrange
            this.studyGroupServiceMock.Setup(service => service
                .GetAllStudyGroupsAsync())
                .ReturnsAsync(this.studyGroups);
            

            // Act
            var result = await this.studyGroupController.GetStudyGroups();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(this.studyGroups);
        }

        [Test]
        public async Task GetStudyGroups_Returns_NotFoundResult_When_No_StudyGroups()
        {
            // Arrange
            List<StudyGroupResponseDto> studyGroup = null;

            this.studyGroupServiceMock.Setup(service => service
                .GetAllStudyGroupsAsync())
                .ReturnsAsync(studyGroup);

            // Act
            var result = await studyGroupController.GetStudyGroups();

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetStudyGroups_Throws_Exception_When_Service_Fails()
        {
            // Arrange
            Exception exception = null;

            this.studyGroupServiceMock.Setup(service => service
                .GetAllStudyGroupsAsync())
                .ThrowsAsync(new Exception("Failed to retrieve study groups"));

            // Act            
            try
            {
                await this.studyGroupController.GetStudyGroups();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Failed to retrieve study groups");
        }

        [Test]
        public async Task GetStudyGroupBySubject_Returns_OkResult_With_StudyGroup()
        {
            // Arrange

            this.studyGroupServiceMock.Setup(service => service.SearchStudyGroupBySubjectAsync("Math"))
                .ReturnsAsync(this.studyGroups);


            // Act
            var result = await this.studyGroupController.GetStudyGroupBySubjectAsync("Math");

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(this.studyGroups);
        }

        [Test]
        public async Task GetStudyGroupBySubject_Returns_NotFoundResult_When_No_StudyGroups()
        {
            // Arrange
            List<StudyGroupResponseDto> studyGroup = null;

            this.studyGroupServiceMock.Setup(service => service
                .SearchStudyGroupBySubjectAsync("Math"))
                .ReturnsAsync(studyGroup);

            // Act
            var result = await studyGroupController.GetStudyGroupBySubjectAsync("Math");

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetStudyGroupBySubject_Throws_Exception_When_Service_Fails()
        {
            // Arrange
            Exception exception = null;

            this.studyGroupServiceMock.Setup(service => service
                .SearchStudyGroupBySubjectAsync("Math"))
                .ThrowsAsync(new Exception("Failed to retrieve study groups"));

            // Act            
            try
            {
                var result = await studyGroupController.GetStudyGroupBySubjectAsync("Math");
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("Failed to retrieve study groups");
        }

        // Same for the rest of the methods
    }
}
