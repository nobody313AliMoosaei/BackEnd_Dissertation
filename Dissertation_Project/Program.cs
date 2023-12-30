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
builder.Services.AddHttpContextAccessor();

#region Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromDays(1);
});
#endregion

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
    options.User.RequireUniqueEmail = false;

    // SignIn
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedAccount = false;

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

builder.Services.AddTransient<BusinessLayer.Services.Email.IEmailSender, BusinessLayer.Services.Email.EmailSender>();

builder.Services.AddTransient<BusinessLayer.Services.JWT.IJWTTokenManager, BusinessLayer.Services.JWT.JWTTokenManager>();

builder.Services.AddTransient<BusinessLayer.Services.Log.ILogManager, BusinessLayer.Services.Log.LogManager>();

builder.Services.AddTransient<BusinessLayer.Services.Log.IHistoryManager, BusinessLayer.Services.Log.HistoryManager>();

builder.Services.AddTransient<BusinessLayer.Services.Session.ISessionManager, BusinessLayer.Services.Session.SessionManager>();

builder.Services.AddTransient<BusinessLayer.Services.UploadFile.IUploadFile, BusinessLayer.Services.UploadFile.UploadFile>();

builder.Services.AddScoped<BusinessLayer.Services.SignUp.SignUpBL>();

builder.Services.AddScoped<BusinessLayer.Services.Dissertation.DissertationBL>();

builder.Services.AddScoped<BusinessLayer.Services.Administrator.AdministratorBL>();

builder.Services.AddScoped<BusinessLayer.Services.EmployeeService.EmployeeService>();

builder.Services.AddTransient<BusinessLayer.Services.Teacher.ITeacherManager, BusinessLayer.Services.Teacher.TeacherManager>();

builder.Services.AddTransient<BusinessLayer.Services.GeneralService.IGeneralService, BusinessLayer.Services.GeneralService.GeneralService>();

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

    option.AddPolicy("AAA", pt =>
    {
        pt.WithOrigins("https://localhost:3000")
        .WithMethods("GET", "POST", "PUT", "DELETE")
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .AllowCredentials();
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

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}


app.UseHttpsRedirection();

app.UseCors("CORS_DEFUALT");
app.UseHangfireDashboard(pathMatch: "/HangfireDashboard");
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();
