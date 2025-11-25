using System.Text;
using FluentValidation;
using MedizID.API.Common;
using MedizID.API.Data;
using MedizID.API.DTOs;
using MedizID.API.Middleware;
using MedizID.API.Models;
using MedizID.API.Repositories;
using MedizID.API.Services;
using MedizID.API.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("logs/mediz-id-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Settings configuration
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>() ?? new JwtSettings
{
    SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "your-super-secret-key-at-least-32-characters-long-for-hmac256",
    Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "mediz.id",
    Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "mediz.id-api",
    ExpirationMinutes = int.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES"), out var exp) ? exp : 1440
};

var openAISettings = builder.Configuration.GetSection("OpenAISettings").Get<OpenAISettings>() ?? new OpenAISettings
{
    ApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? ""
};

var databaseSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>() ?? new DatabaseSettings
{
    ConnectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? 
        $"Host={Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost"};Port={Environment.GetEnvironmentVariable("DB_PORT") ?? "5432"};Database={Environment.GetEnvironmentVariable("DB_NAME") ?? "mediz_db"};Username={Environment.GetEnvironmentVariable("DB_USER") ?? "mediz_user"};Password={Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "mediz_password"};"
};

builder.Services.AddSingleton(jwtSettings);
builder.Services.AddSingleton(openAISettings);
builder.Services.AddSingleton(databaseSettings);

// Database
builder.Services.AddDbContext<MedizIDDbContext>(options =>
    options.UseNpgsql(databaseSettings.ConnectionString));

// Identity
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<MedizIDDbContext>()
    .AddDefaultTokenProviders();

// Authentication
var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);
builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Authorization
builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Services
builder.Services.AddScoped<IAuthService, JwtAuthService>();
builder.Services.AddScoped<IAIService, AIService>();
builder.Services.AddHttpClient();

// Repositories
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<IDrugRepository, DrugRepository>();
builder.Services.AddScoped<IFacilityRepository, FacilityRepository>();
builder.Services.AddScoped<IDiagnosisRepository, DiagnosisRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IAnamnesisRepository, AnamnesisRepository>();

// Business Services
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<IDiagnosisService, DiagnosisService>();
builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
builder.Services.AddScoped<IDrugService, DrugService>();
builder.Services.AddScoped<IFacilityService, FacilityService>();
builder.Services.AddScoped<IAnamnesisService, AnamnesisService>();
builder.Services.AddScoped<IAIRecommendationService, AIRecommendationService>();

// Validation
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "mediz.id API",
        Version = "v1.0.0",
        Description = "Indonesian Clinic Management and Electronic Medical Record System",
        Contact = new()
        {
            Name = "mediz.id Support",
            Email = "support@mediz.id"
        }
    });

    // JWT Authentication in Swagger
    c.AddSecurityDefinition("Bearer", new()
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter JWT token"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new()
            {
                Reference = new()
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Database migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MedizIDDbContext>();
    db.Database.Migrate();
}

// Middleware
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "mediz.id API v1");
    c.RoutePrefix = "swagger/ui";
});

app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Endpoints
app.MapGet("/", () => new
{
    message = "Welcome to mediz.id API",
    version = "1.0.0",
    docs = "/swagger/ui"
}).Produces<ApiResponse>();

app.MapGet("/health", () => new
{
    status = "healthy",
    service = "mediz.id API",
    timestamp = DateTime.UtcNow
}).WithName("HealthCheck").Produces<HealthCheckResponse>();

app.MapControllers();

app.Run();
