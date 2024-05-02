using Data.Repository.StudyGroup;
using EPAM_WEBAPI.Application.Adapters;
using EPAM_WEBAPI.Application.Services;
using EPAM_WEBAPI.Configurations.DbContext;
using EPAM_WEBAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer("Data Source=YOUR_DATASOURCE;Initial Catalog=EPAM;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=true;");
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IStudyGroupService, StudyGroupService>();
builder.Services.AddTransient<IAdaptStudyGroup, AdaptStudyGroup>();
builder.Services.AddTransient<IStudyGroupRepository, StudyGroupRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }