namespace EPAM_WEBAPI.Application.DTO
{
    public class StudyGroupResponseDto
    {
        public StudyGroupResponseDto(string name, string subject, List<string> students, DateTime createDate)
        {
            Name = name;
            Subject = subject;
            Students = students;
            CreateDate = createDate;
        }

        public string Name { get; }

        public string Subject { get; }

        public List<string> Students { get; private set; }

        public DateTime CreateDate { get; }
    }
}
