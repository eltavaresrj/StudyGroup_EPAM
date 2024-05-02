namespace Data.Repository.StudyGroup
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EPAM_WEBAPI.Domain.Model;
    using EPAM_WEBAPI.Configurations.DbContext;
    using EPAM_WEBAPI.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class StudyGroupRepository : IStudyGroupRepository
    {
        private readonly ApplicationDbContext dbContext;

        public StudyGroupRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task CreateStudyGroupAsync(StudyGroup studyGroup)
        {
            if(this.dbContext.StudyGroup.Any(x => x.Subject == studyGroup.Subject)) 
            {
                throw new Exception($"Group for subject {studyGroup.Subject} already exists.");
            }

            dbContext.StudyGroup.Add(studyGroup);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<StudyGroup>> GetAllStudyGroupsAsync()
        {
            return await dbContext.StudyGroup.Include(x => x.Student).ToListAsync();
        }

        public async Task JoinUserToStudyGroupAsync(Guid studentId, Guid studyGroupId)
        {
            this.ValidateJoinGroup(studyGroupId, studentId);

            var studyGroup = await dbContext.StudyGroup.FindAsync(studyGroupId);
            var student = await dbContext.Student.FindAsync(studentId);

            studyGroup?.AddStudent(student);

            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveUserFromStudyGroupAsync(Guid studentId, Guid studyGroupId)
        {
            this.ValidateRemoveGroup(studyGroupId, studentId);

            var studyGroup = await dbContext.StudyGroup.Include(x => x.Student).FirstOrDefaultAsync(s => s.Id == studyGroupId);
            var student = await dbContext.Student.FindAsync(studentId);

            studyGroup?.RemoveStudent(student);

            await dbContext.SaveChangesAsync();
        }

        public async Task<List<StudyGroup>> SearchStudyGroupBySubjectAsync(Subject subject)
        {
            return await dbContext.StudyGroup.Include(x => x.Student)
                .Where(s => s.Subject == subject)
                .ToListAsync();
        }

        private void ValidateGroupAndStudent(Guid groupId, Guid studentId)
        {
            var group = dbContext.StudyGroup.Find(groupId);
            var student = dbContext.Student.Find(studentId);

            if (group == null)
            {
                throw new Exception($"No study group with id {groupId} was found in the database!");
            }

            if (student == null)
            {
                throw new Exception($"No student with id {studentId} was found in the database!");
            }
        }

        private void ValidateJoinGroup(Guid groupId, Guid studentId)
        {
            ValidateGroupAndStudent(groupId, studentId);

            var group = dbContext.StudyGroup.Include(s => s.Student).SingleOrDefault(s => s.Id == groupId);

            if (group.Student.Any(s => s.Id == studentId))
            {
                throw new Exception($"Student with id {studentId} already joined this study group!");
            }
        }

        private void ValidateRemoveGroup(Guid groupId, Guid studentId)
        {
            ValidateGroupAndStudent(groupId, studentId);
        }
    }
}