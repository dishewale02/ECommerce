using ECommerce.Data;
using ECommerce.Models.DataModels;
using ECommerce.Repo.Classes.AuthRepoClasses;
using ECommerce.Repo.Interfaces.AuthRepoInterface;
using ECommerce.Services.Classes.AutoMapperService;
using ECommerce.Services.Classes.RepoServiceClasses.AuthRepoServiceClass;
using ECommerce.Services.Classes.RepoServiceClasses.JwtTokenGeneratorClass;
using ECommerce.Services.Classes.RepoServiceClasses.JwtTokenGeneratorClass.AccessTokenGeneratorClass;
using ECommerce.Services.Classes.RepoServiceClasses.JwtTokenGeneratorClass.RefreshTokenGeneratorClass;
using ECommerce.Services.Classes.RepoServiceClasses.PasswordHasherServiceClass;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.AuthServiceInterface;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.JwtTokenGeneratorInterface;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.PasswordHasherServiceInterface;
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

//Add other Dependency Injection Services.
builder.Services.AddScoped<IAuthRepoService, AuthRepoService>();
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
builder.Services.AddScoped<IAuthenticator, Authenticator>();
builder.Services.AddScoped<AccessTokenGenerator>();
builder.Services.AddScoped<RefreshTokenGenerator>();
builder.Services.AddScoped<TokenWriter>();

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

//configure auto mapper.
builder.Services.AddAutoMapper(typeof(AutoMapperService));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
