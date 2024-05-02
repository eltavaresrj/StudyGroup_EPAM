namespace EPAM_WEBAPI.Tests.Application.Adapters
{
    using NUnit.Framework;
    using FluentAssertions;
    using System;
    using System.Collections.Generic;
    using EPAM_WEBAPI.Domain.Model;
    using EPAM_WEBAPI.Application.DTO;
    using EPAM_WEBAPI.Application.Adapters;

    [TestFixture]
    public class AdaptStudyGroupTests
    {
        private AdaptStudyGroup adapter;

        [SetUp]
        public void Setup()
        {
            adapter = new AdaptStudyGroup();
        }

        // AdaptStudyGroupFromDtoToDomain Tests

        [Test]
        [TestCase("NameXpto", "Math", Subject.Math)]
        [TestCase("NameXpto", "Physics", Subject.Physics)]
        [TestCase("NameXpto", "Chemistry", Subject.Chemistry)]
        [TestCase("NameXpto", "PHYSICS", Subject.Physics)]
        public void AdaptStudyGroupFromDtoToDomain_ValidDto_ReturnsStudyGroup(string name, string subject, Subject expectedSubject)
        {
            // Arrange
            var dto = new StudyGroupDto(name, subject);

            // Act
            var result = adapter.AdaptStudyGroupFromDtoToDomain(dto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(name);
            result.Subject.Should().Be(expectedSubject);
            result.CreatedDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Test]
        [TestCase("NameXpto", "History", "History is not a valid subject!")]
        [TestCase("Name", "Math", "Name must have lenght between 5 and 30!")]
        [TestCase("!NameWithLenghHigherThan30Valid", "Math", "Name must have lenght between 5 and 30!")]
        public void AdaptStudyGroupFromDtoToDomain_InvalidSubject_ThrowsException(string name, string subject, string error)
        {
            // Arrange
            var dto = new StudyGroupDto(name, subject);

            // Act & Assert
            adapter.Invoking(a => a.AdaptStudyGroupFromDtoToDomain(dto))
                .Should().Throw<ArgumentException>().WithMessage(error);
        }

        [Test]
        public void AdaptStudyGroupFromDtoToDomain_NullDto_ReturnsNull()
        {
            // Act
            var result = adapter.AdaptStudyGroupFromDtoToDomain(null);

            // Assert
            result.Should().BeNull();
        }

        // AdaptStudyGroupsFromDomainToDto Tests

        [Test]
        public void AdaptStudyGroupsFromDomainToDto_OneGroupWithStudents_ReturnsDtoWithStudents()
        {
            // Arrange
            var student1 = new Student("S1");
            var student2 = new Student("S2");

            var studyGroup = new StudyGroup("NameXpto", Subject.Math);

            studyGroup.AddStudent(student1);
            studyGroup.AddStudent(student2);

            var studyGroups = new List<StudyGroup>() { studyGroup };

            // Act
            var result = adapter.AdaptStudyGroupsFromDomainToDto(studyGroups);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
            result[0].Name.Should().Be("NameXpto");
            result[0].Subject.Should().Be("Math");
            result[0].Students.Should().ContainInOrder("S1", "S2");
            result[0].CreateDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void AdaptStudyGroupsFromDomainToDto_TwoGroupsWithStudents_ReturnsDtoWithStudents()
        {
            // Arrange
            var student1 = new Student("S1");
            var student2 = new Student("S2");

            var studyGroup = new StudyGroup("NameXpto", Subject.Math);
            var studyGroup2 = new StudyGroup("NameXpto2", Subject.Chemistry);

            studyGroup.AddStudent(student1);
            studyGroup.AddStudent(student2);
            studyGroup2.AddStudent(student1);

            var studyGroups = new List<StudyGroup>() { studyGroup, studyGroup2 };

            // Act
            var result = adapter.AdaptStudyGroupsFromDomainToDto(studyGroups);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result[0].Name.Should().Be("NameXpto");
            result[0].Subject.Should().Be("Math");
            result[0].Students.Should().ContainInOrder("S1", "S2");
            result[0].CreateDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));

            result[1].Name.Should().Be("NameXpto2");
            result[1].Subject.Should().Be("Chemistry");
            result[1].Students.Should().ContainSingle("S1");
            result[1].CreateDate.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Test]
        public void AdaptStudyGroupsFromDomainToDto_NullList_ReturnsNull()
        {
            // Act
            var result = adapter.AdaptStudyGroupsFromDomainToDto(null);

            // Assert
            result.Should().BeNull();
        }

        [Test]
        public void AdaptStudyGroupsFromDomainToDto_EmptyList_ReturnsEmptyDtoList()
        {
            // Arrange
            var studyGroups = new List<StudyGroup>();

            // Act
            var result = adapter.AdaptStudyGroupsFromDomainToDto(studyGroups);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

    }

}
