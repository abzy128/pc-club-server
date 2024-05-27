using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using pc_club_server.Core.Options;
using pc_club_server.Infrastructure.Database;
using pc_club_server.Infrastructure.Mapping;
using pc_club_server.Services.JwtService;
using pc_club_server.Services.PlayerSessionService;
using pc_club_server.Services.SteamService;
using pc_club_server.Services.UserService;
using Serilog;
using Serilog.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var jwtOptionsSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtOptions>(jwtOptionsSection);

builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
});

builder.Services.AddDbContext<PcClubDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("postgresql"));
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var signingKey = Encoding.UTF8.GetBytes(jwtOptionsSection["Key"] ?? throw new Exception("Key not found in appSettings"));

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwtOptionsSection["Issuer"],
        ValidAudience = jwtOptionsSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(signingKey),
        ValidateLifetime = false,
        ValidateAudience = false,
    };
});
builder.Services.AddAuthorization();

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<Mappings>();
});
builder.Services.AddSingleton(mapperConfig.CreateMapper());

builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ISteamService, SteamService>();
builder.Services.AddTransient<IPlaySessionService, PlaySessionService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo 
        { 
            Title = "PC Club API", 
            Version = Environment.Version.ToString(),
        }
    );
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            Array.Empty<string>()
        }
    });
    options.EnableAnnotations();
});

var app = builder.Build();

app.UseCors();

app.UseSerilogRequestLogging();

app.UseSwagger(options =>
{
    options.RouteTemplate = "swagger/{documentName}/swagger.json";
});

app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
