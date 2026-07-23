using InnoClinic.Profiles.Api.Extensions;
using Profiles.Infrastructure.Extensions;
using Profiles.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddKeycloakAuth(builder.Configuration);
builder.Services.AddSwaggerWithAuth(builder.Configuration);

var app = builder.Build();

app.UseWebApplicationPipeline();

app.Run();