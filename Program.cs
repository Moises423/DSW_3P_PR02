using DSW_3P_PR02.Repositories;
using DSW_3P_PR02.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IPersonajeRepository, JsonPersonajeRepository>();
builder.Services.AddScoped<IPersonajeService, PersonajeService>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseStatusCodePages();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
