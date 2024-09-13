using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SQL_API.Context;
using SQL_API.Models;
using SQL_API.Services.MailService.Abstract;
using SQL_API.Services.MailService.Concrete;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers().AddJsonOptions(jsonOpt =>
{
    jsonOpt.JsonSerializerOptions.PropertyNamingPolicy = null;
});
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins",
                          policy =>
                          {
                              policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "192.168.2.168" || new Uri(origin).Host == "192.168.2.13"|| new Uri(origin).Host == "localhost"|| new Uri(origin).Host == "192.168.2.13:86")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>  {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        

    };
    options.MapInboundClaims = false;
});
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("con")));
builder.Services.AddDbContext<NOVAEFECEDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("NOVA_EFECE")));
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("MyAllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
