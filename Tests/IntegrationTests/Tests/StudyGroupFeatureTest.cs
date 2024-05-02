namespace EPAM_WEBAPI.Tests.IntegrationTests.Tests
{
    using EPAM_WEBAPI.Domain.Model;
    using EPAM_WEBAPI.Application.DTO;
    using EPAM_WEBAPI.Configurations.DbContext;
    using EPAM_WEBAPI.Tests;
    using EPAM_WEBAPI.Tests.Engines;
    using Microsoft.Extensions.DependencyInjection;

    [TestFixture]
    internal class StudyGroupFeatureTest
    {
        private StudyGroupArrangeEngine arrange;
        private StudyGroupActEngine act;
        private StudyGroupAssertEngine assert;

        private TestWebApplicationFactory webApplicationFactory;
        private ApplicationDbContext dbContext;

        [OneTimeSetUp]
        public void OneTimeSetup() 
        {
            this.webApplicationFactory = new TestWebApplicationFactory();

            var applicationProvider = this.webApplicationFactory.applicationProvider;

            this.dbContext = applicationProvider.GetService<ApplicationDbContext>();
        }

        [SetUp]
        public void Setup() 
        {
            this.arrange = new StudyGroupArrangeEngine(this.dbContext);
            this.act = new StudyGroupActEngine(this.webApplicationFactory);
            this.assert = new StudyGroupAssertEngine(this.dbContext);

            Thread.Sleep(100);
            this.arrange.TruncateDB();
            this.arrange.SeedStudyGroupTestsPreRequeriments();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            this.dbContext.Dispose();
            this.webApplicationFactory.Dispose();
        }

        [Test]
        [TestCase("MathGroup","Math")]
        [TestCase("PhysicsGroup", "Physics")]
        [TestCase("ChemistryGroup2", "Chemistry")]
        public async Task _1_CreateMathStudyGroup_ReturnStudyGroup_AssertStudyGroupCreated(string name, string subject)
        {
            // Arrange
            var studyGroup = new StudyGroupDto(name, subject);
            var expectedStudyGroup = new StudyGroup(name, (Subject)Enum.Parse(typeof(Subject), subject));

            // Act && Assert
            Assert.DoesNotThrowAsync(async () => await this.act.CreateStudyGroup(studyGroup));
            this.assert.ValidateStudyGroup(expectedStudyGroup);
        }

        [Test]
        public async Task _2_JoinChemistryStudyGroup_ReturnStudyGroup_AssertStudyGroupJoined()
        {
            // Arrange
            var student = this.arrange.GetStudent("Gabriel Tavares");
            var studyGroup = this.arrange.GetStudyGroup("ChemistryGroup");

            var expectedStudyGroup = new StudyGroup("ChemistryGroup", Subject.Chemistry);
            expectedStudyGroup.AddStudent(student);
            studyGroup.Student.ToList().ForEach(expectedStudyGroup.AddStudent);

            // Act && Assert
            Assert.DoesNotThrowAsync(async () => await this.act.JoinStudyGroup(student.Id, studyGroup.Id));
            this.assert.ValidateStudyGroupJoined(expectedStudyGroup);
        }

        [Test]
        public async Task _3_GetAllStudyGroups_ReturnStudyGroups_AssertStudyGroups()
        {
            // Arrange
            var studyGroupExpected = this.arrange.BuildStudyGroupResponse("ChemistryGroup", "Chemistry", new List<string> { "Gabriel Tavares", "Maria Paula" });
            var expectedResponse = new List<StudyGroupResponseDto>() { studyGroupExpected };

            // Act
            var result = await this.act.GetAllStudyGroups();

            // Assert
            this.assert.ValidateStudyGroups(result, expectedResponse);
        }

        [Test]
        public async Task _4_JoinChemistryStudyGroup_ReturnStudyGroup_AssertStudyGroupRemoved()
        {
            // Arrange
            var studyGroup = this.arrange.GetStudyGroup("ChemistryGroup");
            var student = studyGroup.Student.First();

            // Act && Assert
            Assert.DoesNotThrowAsync(async () => await this.act.JoinStudyGroup(student.Id, studyGroup.Id));
            this.assert.ValidateStudyGroupRemoved(student.Id, studyGroup.Id);
        }

        [Test]
        public async Task _5_SearchStudyGroupBySubject_ReturnStudyGroup_AssertStudyGroupReturned()
        {
            // Arrange
            var expectedGroup = new List<StudyGroupResponseDto>()
            {
                new StudyGroupResponseDto("ChemistryGroup", "Chemistry", new List<string> { "Gabriel Tavares", "Maria Paula" }, DateTime.Now)
            };

            // Act
            var response = await this.act.GetBySubject("Chemistry");


            // Assert
            this.assert.ValidateStudyGroups(response, expectedGroup);

        }
    }
}
