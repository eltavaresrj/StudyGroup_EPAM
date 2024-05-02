namespace EPAM_WEBAPI.Tests.Repository
{
    using NUnit.Framework;
    using FluentAssertions;
    using System.Threading.Tasks;
    using Data.Repository.StudyGroup;
    using EPAM_WEBAPI.Configurations.DbContext;
    using EPAM_WEBAPI.Domain.Model;
    using global::Tests.UnitTests.Repository;

    [TestFixture]
    public class StudyGroupRepositoryTests
    {
        private ApplicationDbContext dbContext;
        private StudyGroupRepository repository;

        [SetUp]
        public void Setup()
        {
            this.dbContext = DbContextBuilder.Build();
            this.repository = new StudyGroupRepository(dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        // GetAllStudyGroupsAsync Tests

        [Test, TestCaseSource(nameof(GetAllStudyGroupsTestData))]
        public async Task GetAllStudyGroupsAsync_DbContextReturnsStudyGroupListWithStudents_ReturnsStudyGroupListWithStudents(StudyGroup studyGroup, Student student)
        {
            // Arrange
            if (student != null)
            { 
                studyGroup.AddStudent(student); 
            }

            await this.dbContext.StudyGroup.AddAsync(studyGroup);
            await this.dbContext.SaveChangesAsync();

            // Act
            var result = await this.repository.GetAllStudyGroupsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().ContainEquivalentOf(studyGroup);
        }

        [Test]
        public async Task GetAllStudyGroupsAsync_DbContextReturnsNull_ReturnsShpuldBeNull()
        {
            //Arrange
            var expected = new List<StudyGroup>();

            // Act
            var result = await this.repository.GetAllStudyGroupsAsync();

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task GetAllStudyGroupsAsync_DbContextThrowsException_ReturnsException()
        {
            // Arrange
            this.dbContext.Dispose();
            repository = new StudyGroupRepository(this.dbContext);

            // Act & Assert
            await repository.Invoking(async r => await r.GetAllStudyGroupsAsync())
                            .Should().ThrowAsync<InvalidOperationException>();
        }

        //// SearchStudyGroupBySubjectAsync Tests

        [Test]
        public async Task SearchStudyGroupBySubjectAsync_DbContextReturnsStudyGroups_ReturnsStudyGroupsOfSubjectWithStudents()
        {
            // Arrange
            var mathGroup = new StudyGroup("MathGroup", Subject.Math);
            mathGroup.AddStudent(new Student("Gabriel"));

            var chemistryGroup = new StudyGroup("ChemistryGroup", Subject.Chemistry);
            chemistryGroup.AddStudent(new Student("Gabriel"));

            await dbContext.StudyGroup.AddRangeAsync(mathGroup, chemistryGroup);
            await dbContext.SaveChangesAsync();

            // Act
            var result = await repository.SearchStudyGroupBySubjectAsync(Subject.Math);

            // Assert
            result.Should().NotBeNull();
            result.Should().ContainEquivalentOf(mathGroup);
            result.Should().NotContainEquivalentOf(chemistryGroup);
            result[0].Student.Should().NotBeEmpty();
        }

        //// CreateStudyGroupAsync Tests

        [Test]
        public async Task CreateStudyGroupAsync_ValidStudyGroup_ReturnsOK()
        {
            // Arrange
            var studyGroup = new StudyGroup("Group1", Subject.Math);

            // Act
            Func<Task> action = async () => await repository.CreateStudyGroupAsync(studyGroup);

            // Assert
            await action.Should().NotThrowAsync<Exception>();
            this.dbContext.StudyGroup.Should().NotBeNull();
            this.dbContext.StudyGroup.Should().Contain(studyGroup);
        }

        //// JoinUserToStudyGroupAsync Tests

        [Test]
        public async Task JoinUserToStudyGroupAsync_ValidIds_ReturnsOK()
        {
            // Arrange
            var student = new Student("Gabriel");
            var studyGroup = new StudyGroup("Group1", Subject.Math);
            await dbContext.Student.AddAsync(student);
            await dbContext.StudyGroup.AddAsync(studyGroup);
            await dbContext.SaveChangesAsync();

            // Act
            Func<Task> action = async () => await repository.JoinUserToStudyGroupAsync(student.Id, studyGroup.Id);

            // Assert
            await action.Should().NotThrowAsync<Exception>();

            this.dbContext.StudyGroup.Should().NotBeNull();

            this.dbContext
                .StudyGroup.Single(s => s.Id == studyGroup.Id)
                .Student.Should().Contain(student);
        }

        //// RemoveUserFromStudyGroupAsync Tests

        [Test]
        public async Task RemoveUserFromStudyGroupAsync_ValidIds_ReturnsOK()
        {
            // Arrange
            var student = new Student("Gabriel");
            var studyGroup = new StudyGroup("Group1", Subject.Math);
            studyGroup.AddStudent(student);
            await dbContext.Student.AddAsync(student);
            await dbContext.StudyGroup.AddAsync(studyGroup);
            await dbContext.SaveChangesAsync();

            // Act
            Func<Task> action = async () => await repository.RemoveUserFromStudyGroupAsync(student.Id, studyGroup.Id);

            // Assert
            await action.Should().NotThrowAsync<Exception>();

            this.dbContext
                .StudyGroup.Single(s => s.Id == studyGroup.Id)
                .Student.Should().NotContain(student);
        }

        private static IEnumerable<TestCaseData> GetAllStudyGroupsTestData()
        {
            yield return new TestCaseData(
                new StudyGroup("GroupMath", Subject.Math),
                new Student("Gabriel"))
                .SetName("Return Mapped Group with student");

            yield return new TestCaseData(
                new StudyGroup("GroupMath", Subject.Math),
                null)
                .SetName("Return Mapped Group without student");
        }
    }

}