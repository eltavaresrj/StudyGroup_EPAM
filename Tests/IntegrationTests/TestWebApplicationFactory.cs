namespace EPAM_WEBAPI.Tests
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;

    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        public HttpClient Client { get; private set; }

        public IServiceProvider applicationProvider { get; private set; }      

        public TestWebApplicationFactory() 
        {
            this.Client = this.Server.CreateClient();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();

                applicationProvider = serviceProvider;
            });
        }
    }
}
