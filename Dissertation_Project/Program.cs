using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Hangfire;
using AspNetCore.ReCaptcha;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



#region DataBase Confing
builder.Services.AddDbContext<DataLayer.DataBase.Context_Project>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});
#endregion

#region Identity Confing
builder.Services
    .AddIdentity<DataLayer.Entities.Users, DataLayer.Entities.Roles>()
    .AddEntityFrameworkStores<DataLayer.DataBase.Context_Project>()
    .AddDefaultTokenProviders()
    .AddRoles<DataLayer.Entities.Roles>();


builder.Services.Configure<IdentityOptions>(options =>
{
    // Password Config
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 7;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;

    // User Config
    options.User.RequireUniqueEmail = true;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "";
    options.LoginPath = "/SignUp/Login";
    options.LogoutPath = "/SignUp/Logout";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
});
#endregion

#region JWT Token
builder.Services.AddAuthentication(t =>
{
    t.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    t.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    t.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = builder.Configuration["JWTConfiguration:issuer"],
        ValidAudience = builder.Configuration["JWTConfiguration:audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTConfiguration:key"])),
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true
    };
});
#endregion

#region IOC Container

builder.Services.AddTransient<Dissertation_Project.Core.Utlities.JWT.IJWTBearer,
    Dissertation_Project.Core.Utlities.JWT.JWTBearer>();

builder.Services.AddTransient<Dissertation_Project.Model.Infra.Interfaces.IEmailSender,
    Dissertation_Project.Model.Infra.Managers.EmailSender>();

builder.Services.AddTransient<Dissertation_Project.Model.Infra.Interfaces.ILogManager,
    Dissertation_Project.Model.Infra.Managers.LogManager>();

builder.Services.AddTransient<Dissertation_Project.Model.Infra.Interfaces.IUpload_File,
    Dissertation_Project.Model.Infra.Managers.Upload_File_Implement>();

builder.Services.AddTransient<Dissertation_Project.Model.Infra.Interfaces.IGoogle_Recaptcha,
    Dissertation_Project.Model.Infra.Managers.Google_Recaptcha>();

#endregion

#region HangFire
builder.Services.AddHangfire(config =>
{
    config.
    SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(builder
        .Configuration.GetConnectionString("HangFireConnection"));
});

builder.Services.AddHangfireServer();

#endregion

#region Versioning
builder.Services.AddApiVersioning(option =>
{
    option.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    option.AssumeDefaultVersionWhenUnspecified = true;
    option.ReportApiVersions = true;
});
#endregion

#region CORS
builder.Services.AddCors(option =>
{
    option.AddPolicy("CORS_DEFUALT", pt =>
    {
        pt.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

#endregion

#region reCAPTCHA
builder.Services.AddReCaptcha(option =>
{
    option.SiteKey = builder.Configuration["reCAPTCHA:SITKEY"];
    option.SecretKey = builder.Configuration["reCAPTCHA:SECRETKEY"];
});
#endregion


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors("CORS_DEFUALT");
app.UseHangfireDashboard(pathMatch: "/HangfireDashboard");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
