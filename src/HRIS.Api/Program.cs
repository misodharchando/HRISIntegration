using System.Text;
using HRIS.Application;
using HRIS.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var b = WebApplication.CreateBuilder(args);
b.Services.AddControllers();
b.Services.AddApplication().AddInfrastructure(b.Configuration);
b.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
        o.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = b.Configuration["Jwt:Issuer"],
            ValidAudience = b.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(b.Configuration["Jwt:Key"]!)
            ),
        }
    );
b.Services.AddAuthorization();
b.Services.AddHealthChecks();
b.Services.AddEndpointsApiExplorer();
b.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "HRIS API", Version = "v1" });
    c.AddSecurityDefinition(
        "Bearer",
        new()
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
        }
    );
    c.AddSecurityRequirement(
        new()
        {
            {
                new()
                {
                    Reference = new() { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                },
                []
            },
        }
    );
});
var app = b.Build();
app.UseExceptionHandler("/error");
app.Map("/error", () => Results.Problem());
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");
app.Run();

public partial class Program { }
