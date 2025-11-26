using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TakeHomeAssignment.General.AccountAPI.Middlewares;
using TakeHomeAssignment.General.AccountAPI.Model.General;
using TakeHomeAssignment.General.AccountAPI.Repository;
using TakeHomeAssignment.General.AccountAPI.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env}.json", optional: true);
builder.Services.Configure<ApplicationSetting>(builder.Configuration.GetSection("ApplicationSetting"));

var jwtKey = Environment.GetEnvironmentVariable("THA_JWT_SIGNING_KEY");
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

if (string.IsNullOrEmpty(jwtKey))
    throw new Exception("Missing Environment Variable: JWT_SIGNING_KEY");

builder.Services.AddCors(o => o.AddPolicy(name: "AllowOrigin", builder =>
{
    builder.WithOrigins(
        "https://localhost:3000"
        )
        .SetIsOriginAllowedToAllowWildcardSubdomains()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
}));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // SLIDING EXPIRATION (15 minutes inactivity)
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // allow token from header only
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                // Extend token if needed (logic optional)
                return Task.CompletedTask;
            }
        };

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true
        };
    });

// Register Auth service
builder.Services.AddSingleton<IAuthRepository, AuthRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<InactivityMiddleware>();
app.MapControllers();

app.Run();
