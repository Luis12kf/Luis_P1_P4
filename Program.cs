using Scalar.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Luis_P1_P4.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<PersonaServices>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var personaServices = scope.ServiceProvider.GetRequiredService<PersonaServices>();
    await personaServices.InitializeAsync();
}

// Configure the HTTP request pipeline.
    app.MapOpenApi();
    app.MapScalarApiReference();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
