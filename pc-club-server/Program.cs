using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using pc_club_server.Core.Options;
using pc_club_server.Infrastructure.Database;
using pc_club_server.Services.JwtService;
using pc_club_server.Services.UserService;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
