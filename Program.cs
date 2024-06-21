using Microsoft.EntityFrameworkCore;
using SQL_API.Context;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins",
                          policy =>
                          {
                              policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "192.168.2.13"|| new Uri(origin).Host == "localhost")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("con")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("MyAllowSpecificOrigins");
app.UseAuthorization();

app.MapControllers();

app.Run();
