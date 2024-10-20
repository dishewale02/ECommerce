using ECommerce.Data;
using ECommerce.Models.DataModels;
using ECommerce.Models.InputModelsDTO.EmailSenderDTO;
using ECommerce.Services;
using ECommerce.Services.Classes.RepoServiceClasses.EmailServiceClass;
using ECommerce.Services.Interfaces.OtherServicesInterfaces.EmailServiceInterface;
using ECommerce.WebAPI.ApplicationConstant;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//Add dbContext.
var connectionString = builder.Configuration.GetConnectionString(ApplicationConstants.AUTH_DB_CONNECTION);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));

builder.Services.AddCommonServices();

//configure email settings.
var emailConfig = builder.Configuration.GetSection(ApplicationConstants.AUTH_EMAIL_SETTINGS).Get<EmailSettings>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddSingleton<IEmailService, EmailService>();

//allow cors policies.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigin",
        builder => builder.AllowAnyOrigin()//.WithOrigins("http://localhost:5173/") // Replace with your frontend URL
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

//get connection string from appsetting.json for authentication.
AuthenticationConfig _authConfig = new AuthenticationConfig();

//bind the appsetting.json stirng object to instance created above.
builder.Configuration.Bind(ApplicationConstants.AUTH_CONNECTION_STRING_JWT, _authConfig);

builder.Services.AddSingleton(_authConfig);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _authConfig.Issuer,
                ValidAudience = _authConfig.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authConfig.AccessTokenSectet)),
                ClockSkew = TimeSpan.FromSeconds(0)
            };
        });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//use cors policies.
app.UseCors("AllowAllOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//use static Files.
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
