namespace EPAM_WEBAPI.Domain.Model
{
    public class Student
    {
        public Student() { }

        public Student(string name) 
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public virtual ICollection<StudyGroup> StudyGroup { get; set; } // Navigation property
    }
}
