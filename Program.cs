using DSW_3P_PR02.Data;
using DSW_3P_PR02.Repositories;
using DSW_3P_PR02.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPersonajeRepository, EfPersonajeRepository>();
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
