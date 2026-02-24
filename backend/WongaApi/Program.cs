using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using WongaApi.Data;
using WongaApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();

// ✅ Swagger/OpenAPI for .NET 8
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var jwtSecret = builder.Configuration["JWT_SECRET"];
if (string.IsNullOrWhiteSpace(jwtSecret))
    throw new InvalidOperationException("JWT_SECRET environment variable is not set.");


builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),

            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(5),

            NameClaimType = ClaimTypes.Email,
            RoleClaimType = ClaimTypes.Role
        };


    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// ✅ WAIT FOR DB + MIGRATE WITH RETRIES
await ApplyMigrationsWithRetryAsync(app);

// ✅ Swagger UI (Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static async Task ApplyMigrationsWithRetryAsync(WebApplication app)
{
    const int maxRetries = 10;
    const int delaySeconds = 3;

    for (int attempt = 1; attempt <= maxRetries; attempt++)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await db.Database.MigrateAsync();
            return;
        }
        catch (PostgresException)
        {
            // Database not ready yet
        }
        catch (NpgsqlException)
        {
            // Database connection failed
        }
        catch (Exception)
        {
            // Other database errors
        }

        if (attempt < maxRetries)
            await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
    }

    throw new Exception("Database was not ready after multiple retries. Migration failed.");
}