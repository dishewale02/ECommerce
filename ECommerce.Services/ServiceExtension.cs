using ECommerce.Repo.Classes.AuthRepoClasses;
using ECommerce.Repo.Interfaces.AuthRepoInterface;
using ECommerce.Services.Classes.RepoServiceClasses.AuthRepoServiceClass;
using ECommerce.Services.Classes.RepoServiceClasses.GenericRepoServiceClass;
using ECommerce.Services.Classes.RepoServiceClasses.JwtTokenGeneratorClass.AccessTokenGeneratorClass;
using ECommerce.Services.Classes.RepoServiceClasses.JwtTokenGeneratorClass.RefreshTokenGeneratorClass;
using ECommerce.Services.Classes.RepoServiceClasses.JwtTokenGeneratorClass;
using ECommerce.Services.Classes.RepoServiceClasses.PasswordHasherServiceClass;
using ECommerce.Services.Interfaces.OtherServicesInterfaces.JwtTokenGeneratorInterface;
using ECommerce.Services.Interfaces.OtherServicesInterfaces.PasswordHasherServiceInterface;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.AuthServiceInterface;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.GenericRepoServiceInterface;
using Microsoft.Extensions.DependencyInjection;
using ECommerce.Services.Classes.AutoMapperService;
using ECommerce.Repo.Interfaces.GenericRepoInterface;
using ECommerce.Repo.Classes.GenericRepoClass;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.ProductRepoServiceInterface;
using ECommerce.Services.Classes.RepoServiceClasses.ProductRepoServiceClass;
using ECommerce.Repo.Interfaces.ProductRepoInterface;
using ECommerce.Repo.Classes.ProductRepoClass;
using ECommerce.Services.Interfaces.RepoServiceInterfaces.CategoryRepoServiceInterface;
using ECommerce.Services.Classes.RepoServiceClasses.CategoryRepoServiceClass;
using ECommerce.Repo.Interfaces.CategoryRepoInterface;
using ECommerce.Repo.Classes.CategoryRepoClass;

namespace ECommerce.Services
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddCommonServices(this IServiceCollection services)
        {
            //Add other Dependency Injection Services.
            services.AddScoped<IAuthRepoService, AuthRepoService>();
            services.AddScoped<IAuthRepo, AuthRepo>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<IAuthenticator, Authenticator>();
            services.AddScoped<AccessTokenGenerator>();
            services.AddScoped<RefreshTokenGenerator>();
            services.AddScoped<TokenWriter>();
            services.AddScoped<Validator>();
            services.AddScoped<AdminRepoService>();
            services.AddScoped<IProductRepoService, ProductRepoService>();
            services.AddScoped<ICategoryRepoService, CategoryRepoService>();
            services.AddScoped<IProductRepo, ProductRepo>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();

            // Register generic repository and service
            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            services.AddScoped(typeof(IGenericRepoService<,>), typeof(GenericRepoService<,>));

            //configure auto Mapper.
            services.AddAutoMapper(typeof(AutoMapperService));
            return services;
        }

    }
}
