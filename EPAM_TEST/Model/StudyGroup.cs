namespace EPAM_WEBAPI.Domain.Model
{
    public class StudyGroup
    {
        public StudyGroup() { }

        public StudyGroup(string Name, Subject subject)
        {
            this.Id = Guid.NewGuid();
            this.Name = Name;
            this.Subject = subject;
            this.CreatedDate = DateTime.Now;

            ValidateName();
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public Subject Subject { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public virtual ICollection<Student> Student { get; private set; } // Navigation property

        public void AddStudent(Student student) 
        {
            if (Student == null)
            {
                Student = new List<Student>();
            }

            Student.Add(student);

        }

        public void RemoveStudent(Student student)
        {
            Student.Remove(student);
        }

        private void ValidateName() 
        {
            if(this.Name.Length < 5 || this.Name.Length > 30) 
            {
                throw new ArgumentException("Name must have lenght between 5 and 30!");
            }
        }
    }
}
