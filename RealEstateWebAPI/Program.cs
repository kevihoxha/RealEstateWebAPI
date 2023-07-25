using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PdfSharp.Charting;
using RealEstateWebAPI.BLL;
using RealEstateWebAPI.DAL;
using RealEstateWebAPI.JWTMangament;
using RealEstateWebAPI.Middleware;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
//regjistron si transient servisin per GlobalErrorHandlingMiddleware
// do te krijoje nje instance te re te servisit cdo here qe do te thirret ne cdo pjese te programit
builder.Services.AddTransient<GlobalErrorHandlingMiddleware>();
//identifikon dhe krijon automatisht instancat e kontrollerëve të nevojshëm në varësi të kërkesave HTTP që vijnë në aplikacion
builder.Services.AddControllers();
//lidhja e backend me frontend 
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//konfigurimi i swagger 
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    //konfigurimi i sigurise ne swagger duke perdorur JWT
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    //konfigurimi i sigurise ne swagger , duke kerkuar qe cdo endpoint te kete token
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
//rregjistrimi i TokenService si scoped
builder.Services.AddScoped<TokenService, TokenService>();
// perdoret per te aksesuar http requests ose responses per te marre komponete te ndryshem brenda tyre
builder.Services.AddHttpContextAccessor();
//rregjistrimi i serviseve nga shtresat e tjera BLL dhe DAL 
builder.Services.RegisterServices(builder.Configuration);
//konfiguron sistemin e authentikimit te JWT
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //pergjegjes per validimin dhe menaxhimin e tokenit 
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            // koha e validimit te tokenit te jete precize dhe mos kete zvarritje 
            ClockSkew = TimeSpan.Zero,
            //përcaktojnë se cilat aspekte të tokenit duhet të validohen
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
//konfigurimi per ruajtjen e logeve me ane te SeriLog
Log.Logger = new LoggerConfiguration()
    //percakton nivelin minimal te logeve qe do te ruhen , cdogje poshte Error level nuk do te ruhet
            .MinimumLevel.Error()
            //ben shkrimin e logeve ne nje file
            .WriteTo.File("C:\\Users\\pc\\source\\repos\\RealEstateWebAPI\\RealEstateWebAPI\\bin\\Debug\\net6.0\\Logs\\logs.txt")
            //ben shkrimin e logeve ne databaze ne tabelen Logs
            .WriteTo.MSSqlServer(
        connectionString: "Data Source=.,1401;Initial Catalog=RealEstateWebAPIDb;Persist Security Info=True;User ID=sa;Password=yourStrong(!)Password;TrustServerCertificate=True", // Replace with your actual connection string
        //nese nuk e gjen tabelen Logs ne databaze do ta krijoje ate 
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true,
        }
          ).CreateLogger();
// Log per testim
Log.Warning("This is an information log entry.");
//konfiguron regjistrimin e log-ve përmes Serilog
builder.Services.AddLogging(loggingBuilder =>
{
    //fshin te gjithe providers qe mund te ruanin loget deri tani
    loggingBuilder.ClearProviders();
    //shton serilog si provider kryesor dhe i vetem per menaxhimin e logeve
    //servisi i serilog do te behet dispose nese ai nuk perdoret
    loggingBuilder.AddSerilog(dispose: true);
});
//rregjistrimi i servisit singleton per AuthenticationMiddleware
builder.Services.AddSingleton<AuthenticationMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// vendosja e GlobalErrorHandlingMiddleware ne pipeline ne fillim per te kapur erroret dhe mos vazhduar ne pipeline ne kapen
app.UseMiddleware<GlobalErrorHandlingMiddleware>();
//lidhja e backend me frontend 
app.UseCors();
// perdor sistemin Routing te drejtoje kerkesat HTTP ne kontrollerat perkates 
app.UseRouting();
// inicializimi i databazes 
StartUp.SeedData(app);
//krijon kerkesat HTTPS nga HTTP qe ishin me pare
app.UseHttpsRedirection();
// perdorimi i AuthenticationMiddleware(globalisht) per te kontrolluar nese user eshte i authntikuar
//do te kapi cdo request pervec atyre te dekoruar me atributin AllowAnonymous
app.UseMiddleware(typeof(AuthenticationMiddleware));
//konfiguron si te routohen kerkesat HTTP ne controllers te aplikacionit
app.UseEndpoints(endpoints =>
{
    //kerkesat HTTP tu drejtohen Controllerave perkates ne app
    endpoints.MapControllers();
});

app.Run();