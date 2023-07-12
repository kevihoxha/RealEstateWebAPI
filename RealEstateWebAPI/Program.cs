using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealEstateWebAPI.ActionFilters;
using RealEstateWebAPI.BLL;
using RealEstateWebAPI.Common;
using RealEstateWebAPI.DAL;
using RealEstateWebAPI.JWTMangament;
using RealEstateWebAPI.Middleware;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Formatting.Json;
using Serilog.Sinks.MSSqlServer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddTransient<GlobalErrorHandlingMiddleware>();

builder.Services.AddControllers();

;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddScoped<TokenService, TokenService>();
builder.Services.AddHttpContextAccessor();
builder.Services.RegisterServices(builder.Configuration);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "apiWithAuthBackend",
            ValidAudience = "apiWithAuthBackend",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("!SomethingSecret!12345abcd ergijnewr orwjngkjebwrkg reijbgkewbgrkhwberg")
            ),
        };
    });

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Error()
            .WriteTo.File("C:\\Users\\pc\\source\\repos\\RealEstateWebAPI\\RealEstateWebAPI\\bin\\Debug\\net6.0\\Logs\\logs.txt")
            .WriteTo.MSSqlServer(
        connectionString: "Data Source=.,1401;Initial Catalog=RealEstateWebAPIDb;Persist Security Info=True;User ID=sa;Password=yourStrong(!)Password;TrustServerCertificate=True", // Replace with your actual connection string
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true,
        }
          ).CreateLogger();
Log.Warning("This is an information log entry.");

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(dispose: true);
});

builder.Services.AddSingleton<AuthenticationMiddleware>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalErrorHandlingMiddleware>();
app.UseRouting();
StartUp.SeedData(app);
app.UseHttpsRedirection();
/*app.UseAuthentication();*/
app.UseMiddleware(typeof(AuthenticationMiddleware));
/*app.UseAuthorization();*/

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute(
        name: "errorHandler",
        pattern: "/api/errorHandler",
        defaults: new { controller = "ErrorHandler", action = "HandleError" }
    );
});
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();