using AutoMapper;
using ECommerce.Data;
using ECommerce.Repo.Classes.AuthRepoClasses;
using ECommerce.Repo.Interfaces.AuthRepoInterface;
using ECommerce.Services.RepoServiceClasses.AuthRepoServiceClass;
using ECommerce.Services.RepoServiceInterfaces.AuthRepoServiceInterface;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Services.HasherService;
using ShoppingCart.Services.MapperService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

//Add dbContext.
var connectionString = builder.Configuration.GetConnectionString("DefaultString");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));

//Add other Dependency Injection Services.
builder.Services.AddScoped<IAuthRepoService, AuthRepoService>();
builder.Services.AddScoped<IAuthRepo, AuthRepo>();
builder.Services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

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
