using System.Text;
using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using LMS.API.Middleware;
using LMS.API.Services;
using LMS.Application;
using LMS.Application.Common.Interfaces;
using LMS.Infrastructure.BackgroundServices;
using LMS.Infrastructure.Caching;
using LMS.Infrastructure.Configuration;
using LMS.Infrastructure.Identity;
using LMS.Infrastructure.Messaging;
using LMS.Infrastructure.Persistence;
using LMS.Infrastructure.Persistence.Repositories;
using LMS.Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// ----- Serilog: Console (coloured) + rolling daily File sink -----
builder.Host.UseSerilog((context, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/lms-.log", rollingInterval: RollingInterval.Day));

// ----- Options binding -----
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.Configure<BlobStorageOptions>(builder.Configuration.GetSection(BlobStorageOptions.SectionName));
builder.Services.Configure<ServiceBusOptions>(builder.Configuration.GetSection(ServiceBusOptions.SectionName));
builder.Services.Configure<OverdueCheckerOptions>(builder.Configuration.GetSection(OverdueCheckerOptions.SectionName));

// ----- Application layer (MediatR, AutoMapper, FluentValidation, pipeline behaviour) -----
builder.Services.AddApplication();

// ----- Persistence: EF Core + repositories + unit of work -----
builder.Services.AddDbContext<LmsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<DataSeeder>();

// ----- Redis (cache + refresh-token revocation set) -----
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis")!));
builder.Services.AddScoped<ICacheService, RedisCacheService>();

// ----- Azure Blob Storage (book covers) -----
builder.Services.AddSingleton(sp =>
    new BlobServiceClient(sp.GetRequiredService<IOptions<BlobStorageOptions>>().Value.ConnectionString));
builder.Services.AddScoped<ICoverImageService, AzureBlobCoverImageService>();

// ----- Azure Service Bus (overdue alerts) -----
builder.Services.AddSingleton(sp =>
    new ServiceBusClient(sp.GetRequiredService<IOptions<ServiceBusOptions>>().Value.ConnectionString));
builder.Services.AddScoped<IOverdueAlertPublisher, ServiceBusOverdueAlertPublisher>();

// ----- Identity / tokens & current user -----
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// ----- Background services -----
builder.Services.AddScoped<IOverdueCheckerService, OverdueCheckerService>();
builder.Services.AddHostedService<OverdueCheckerHostedService>();
builder.Services.AddHostedService<OverdueAlertConsumerService>();

// NOTE: skeleton phase — background-service stubs throw NotImplementedException.
// Ignore their faults so the host keeps running until they are implemented.
builder.Services.Configure<HostOptions>(o =>
    o.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore);

// ----- JWT bearer authentication -----
var jwt = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt.Issuer,
            ValidAudience = jwt.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt.SigningKey.Length >= 32 ? jwt.SigningKey : new string('x', 32)))
        };
    });
builder.Services.AddAuthorization();

// ----- Controllers + Swagger (with JWT bearer scheme) -----
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "LMS API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter the JWT access token."
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ----- Pipeline (exception handler first) -----
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ----- Seed baseline data on startup -----
using (var scope = app.Services.CreateScope())
{
    try
    {
        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await seeder.SeedAsync();
    }
    catch (Exception ex)
    {
        Log.Warning(ex, "Data seeding skipped (not yet implemented or infrastructure unavailable).");
    }
}

app.Run();
