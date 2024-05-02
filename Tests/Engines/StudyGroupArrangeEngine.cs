namespace EPAM_WEBAPI.Tests.Engines
{
    using System.Collections.Generic;
    using EPAM_WEBAPI.Domain.Model;
    using EPAM_WEBAPI.Application.DTO;
    using EPAM_WEBAPI.Configurations.DbContext;
    using Microsoft.EntityFrameworkCore;

    internal class StudyGroupArrangeEngine
    {
        private ApplicationDbContext dbContext;

        public StudyGroupArrangeEngine(ApplicationDbContext dbContext) 
        {
            this.dbContext = dbContext;
        }

        public StudyGroup BuildStudyGroup(string name, string subject) 
        {
            var convertedSubject = (Subject)Enum.Parse(typeof(Subject), subject, true);

            return new StudyGroup(name, convertedSubject);
        }

        public StudyGroupResponseDto BuildStudyGroupResponse(string name, string subject, List<string> students = null)
        {
            return new StudyGroupResponseDto(name, subject, students, DateTime.Now);
        }

        public void SeedStudyGroupTestsPreRequeriments() 
        {
            var gabriel = new Student("Gabriel Tavares");
            var maria = new Student("Maria Paula");
            var joao = new Student("Joao Dantas");

            var students = new List<Student>() { maria, gabriel };

            var studyGroup = new StudyGroup("ChemistryGroup", Subject.Chemistry);
            students.ForEach(studyGroup.AddStudent);

            this.dbContext.Student.AddRange(gabriel, maria, joao);
            this.dbContext.StudyGroup.Add(studyGroup);
            this.dbContext.SaveChangesAsync();
        }

        public void SeedStudyGroups(List<StudyGroup> studyGroups) 
        {
            this.dbContext.StudyGroup.AddRange(studyGroups);
            this.dbContext.SaveChangesAsync();
        }

        public void SeedStudyGroup(StudyGroup studyGroup)
        {
            this.dbContext.StudyGroup.Add(studyGroup);
            this.dbContext.SaveChangesAsync();
        }

        public void SeedStudent(Student student)
        {
            this.dbContext.Student.Add(student);
            this.dbContext.SaveChangesAsync();
        }

        public Student GetStudent(string name)
        {
            return this.dbContext.Student.SingleOrDefault(s => s.Name == name);
        }

        public StudyGroup GetStudyGroup(string name)
        {
            return this.dbContext.StudyGroup.SingleOrDefault(s => s.Name == name);
        }

        public void TruncateDB() 
        {
            this.dbContext.Database.ExecuteSqlRaw("DELETE FROM StudentStudyGroup");

            this.dbContext.StudyGroup.RemoveRange(this.dbContext.StudyGroup);

            this.dbContext.Student.RemoveRange(this.dbContext.Student);

            this.dbContext.SaveChangesAsync();
        }
    }
}
