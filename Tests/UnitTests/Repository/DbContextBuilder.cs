namespace Tests.UnitTests.Repository
{
    using EPAM_WEBAPI.Configurations.DbContext;
    using Microsoft.EntityFrameworkCore;

    public static class DbContextBuilder
    {
        public static ApplicationDbContext Build()
        {
            var dbname = "StudyGroup" + Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbname)
                .Options;

            var dbContext = new ApplicationDbContext(options);

            return dbContext;
        }
    }
}
