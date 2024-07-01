using Microsoft.EntityFrameworkCore;
using otomrelationship.Automapper;
using otomrelationship.context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<TestDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("relationshiponetomany")));

// Register AutoMapper with the specific profile
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(
   options =>
   {
       options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
       options.RoutePrefix = string.Empty;
       options.DocumentTitle = "My Swagger";

   }
);

app.Run();
