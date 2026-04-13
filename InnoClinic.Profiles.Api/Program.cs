using FluentValidation;
using InnoClinic.Profiles.Api.Abstractions;
using InnoClinic.Profiles.Api.Services;
using InnoClinic.Profiles.Api.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IPasswordGenerator, PasswordService>();
builder.Services.AddScoped<IEmailService, TempEmailService>();
builder.Services.AddScoped<IReceptionistService, ReceptionistService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateReceptionistRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateReceptionistRequestValidator>();

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
