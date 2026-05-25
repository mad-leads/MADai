using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Asp.Versioning;
using Hangfire;
using Hangfire.SqlServer;
using HealthChecks.UI.Client;
using MADai.Api.Authentication;
using MADai.Api.BackgroundServices;
using MADai.Api.Hubs;
using MADai.Api.Middleware;
using MADai.Api.SignalR;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Infrastructure;
using MADai.Infrastructure.Options;
using MADai.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext();
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddSingleton<IEventPublisher, SignalREventPublisher>();
builder.Services.AddHttpClient("webhook");
builder.Services.AddHostedService<WebhookDeliveryWorker>();

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    opts.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = builder.Environment.IsDevelopment();
    options.MaximumReceiveMessageSize = 1024 * 1024;
});

builder.Services
    .AddApiVersioning(opts =>
    {
        opts.DefaultApiVersion = new ApiVersion(1, 0);
        opts.AssumeDefaultVersionWhenUnspecified = true;
        opts.ReportApiVersions = true;
    })
    .AddApiExplorer(opts =>
    {
        opts.GroupNameFormat = "'v'VVV";
        opts.SubstituteApiVersionInUrl = true;
    });

var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions
{
    Secret = "DEV_ONLY_SECRET_MUST_BE_LONG_FOR_HMAC_SHA256_REPLACE_ME_PLEASE_____"
};
if (string.IsNullOrWhiteSpace(jwt.Secret))
{
    jwt.Secret = "DEV_ONLY_SECRET_MUST_BE_LONG_FOR_HMAC_SHA256_REPLACE_ME_PLEASE_____";
}

builder.Services
    .AddAuthentication(opts =>
    {
        opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.RequireHttpsMetadata = !builder.Environment.IsDevelopment();
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                StringValues accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    })
    .AddScheme<WorkerAuthOptions, WorkerAuthHandler>("Worker", _ => { });

builder.Services.AddAuthorization(opts =>
{
    opts.AddPolicy("WorkerOnly", policy =>
    {
        policy.AddAuthenticationSchemes("Worker").RequireAuthenticatedUser();
    });
});

builder.Services.AddCors(opts =>
{
    var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>()
        ?? ["http://localhost:4211"];

    opts.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins(origins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MADai API",
        Version = "v1"
    });
    opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Bearer token"
    });
    opts.AddSecurityDefinition("WorkerKey", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.ApiKey,
        Name = "X-API-Key",
        In = ParameterLocation.Header,
        Description = "Worker API key"
    });
    opts.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            []
        }
    });
});

var checks = builder.Services.AddHealthChecks();
var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(connectionString))
{
    connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=madai;Trusted_Connection=True;TrustServerCertificate=True;";
}
checks.AddSqlServer(connectionString, "SELECT 1;", name: "sql", tags: ["ready"]);

var hangfireConn = builder.Configuration.GetConnectionString("Hangfire");
if (string.IsNullOrWhiteSpace(hangfireConn))
{
    hangfireConn = builder.Configuration.GetConnectionString("Default");
}

if (string.IsNullOrWhiteSpace(hangfireConn))
{
    hangfireConn = "Server=(localdb)\\MSSQLLocalDB;Database=madaihangfire;Trusted_Connection=True;TrustServerCertificate=True;";
}

builder.Services.AddHangfire(cfg =>
{
    cfg.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(hangfireConn, new SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        });
});
builder.Services.AddHangfireServer();

var app = builder.Build();

app.UseMiddleware<VerbTunnelMiddleware>();
app.UseHttpMethodOverride();
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = [new HangfireDashboardAuthFilter()]
});

app.MapControllers();
app.MapHub<TasksHub>("/hubs/tasks");
app.MapHub<WorkersHub>("/hubs/workers");
app.MapHub<NotificationsHub>("/hubs/notifications");
app.MapHub<DashboardHub>("/hubs/dashboard");
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = registration => registration.Tags.Contains("ready"),
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

try
{
    await DataSeeder.SeedAsync(app.Services);
}
catch (Exception ex)
{
    Log.Logger.Error(ex, "Database seeding failed at startup. Continuing.");
}

app.Run();
