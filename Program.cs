using FluentValidation;
using InnoClinic.Profiles.Api.Abstractions;
using InnoClinic.Profiles.Api.Services;
using InnoClinic.Profiles.Api.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IPatientService, PatientService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreatePatientRequestValidator>();


builder.Services.AddDbContext<ProfilesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
