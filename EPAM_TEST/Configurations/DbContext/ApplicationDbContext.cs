namespace EPAM_WEBAPI.Configurations.DbContext
{
    using EPAM_WEBAPI.Domain.Model;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Student { get; set; }
        public DbSet<StudyGroup> StudyGroup { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Student>()
                .HasMany(u => u.StudyGroup)
                .WithMany(s => s.Student)
                .UsingEntity(j => j.ToTable("StudentStudyGroup"));

            modelBuilder.Entity<StudyGroup>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<StudyGroup>()
                .HasIndex(s => s.Subject)                
                .IsUnique();

            modelBuilder.Entity<StudyGroup>()
            .Property(s => s.Name)
            .HasMaxLength(30)
            .IsRequired();

            modelBuilder.Entity<StudyGroup>()
                .HasMany(u => u.Student)
                .WithMany(s => s.StudyGroup)
                .UsingEntity(j => j.ToTable("StudentStudyGroup"));
        }
    }

}
