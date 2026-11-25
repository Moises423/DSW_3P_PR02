using DSW_3P_PR02.Data;
using DSW_3P_PR02.Repositories;
using DSW_3P_PR02.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

const string CorsPolicyName = "Frontend";

var builder = WebApplication.CreateBuilder(args);

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>();
if (allowedOrigins is null
    || allowedOrigins.Length == 0
    || allowedOrigins.Any(string.IsNullOrWhiteSpace))
{
    throw new InvalidOperationException(
        "Configura al menos un origen válido en Cors:AllowedOrigins.");
}

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
var expireMinutes = builder.Configuration.GetValue<int?>("Jwt:ExpireMinutes");
var demoUsername = builder.Configuration["DemoCredentials:Username"];
var demoPassword = builder.Configuration["DemoCredentials:Password"];

if (string.IsNullOrWhiteSpace(jwtKey) || jwtKey.Length < 32)
{
    throw new InvalidOperationException(
        "Configura Jwt:Key en User Secrets o en una variable de entorno; debe tener al menos 32 caracteres.");
}

if (string.IsNullOrWhiteSpace(jwtIssuer)
    || string.IsNullOrWhiteSpace(jwtAudience)
    || expireMinutes is null or <= 0)
{
    throw new InvalidOperationException(
        "Configura Jwt:Issuer, Jwt:Audience y un Jwt:ExpireMinutes mayor que cero.");
}

if (string.IsNullOrWhiteSpace(demoUsername) || string.IsNullOrWhiteSpace(demoPassword))
{
    throw new InvalidOperationException(
        "Configura DemoCredentials:Username y DemoCredentials:Password en User Secrets o en variables de entorno.");
}

var jwtSettings = new JwtSettings(jwtKey, jwtIssuer, jwtAudience, expireMinutes.Value);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
    options.AddPolicy(CorsPolicyName, policy =>
        policy
            .WithOrigins(allowedOrigins)
            .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
            .WithHeaders("Content-Type", "Authorization")));
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CharacterHub API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Ingresa el token JWT con el esquema Bearer."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
    });
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPersonajeRepository, EfPersonajeRepository>();
builder.Services.AddScoped<IPersonajeService, PersonajeService>();
builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton(new DemoCredentials(demoUsername, demoPassword));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseCors(CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CharacterHub API v1"));
}

app.MapControllers();

app.Run();
