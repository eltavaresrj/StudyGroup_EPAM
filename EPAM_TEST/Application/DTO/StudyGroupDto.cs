namespace EPAM_WEBAPI.Application.DTO
{
    using System.ComponentModel.DataAnnotations;

    public class StudyGroupDto
    {
        public StudyGroupDto(string name, string subject)
        {
            Name = name;
            Subject = subject;
        }

        [Required]
        public string Name { get; }

        [Required]
        public string Subject { get; }
    }
}
