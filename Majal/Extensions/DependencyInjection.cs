using FluentValidation.AspNetCore;
using Majal.Core.Interfaces.Repositories;
using Majal.Core.Interfaces.Service;
using Majal.Core.UnitOfWork;
using Majal.Repository.Persistence;
using Majal.Repository.Repositories;
using Majal.Repository.UnitOfWork;
using System.Reflection;
using Majal.Core.Abstractions;

namespace Majal.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();


            services.AddDbContextConfig(configuration);

            services.AddSwaggerGen()
                    .AddEndpointsApiExplorer()
                    .AddExceptionHandlerConfig()
                    .AddMapsterConfig()
                    .AddFluentValidationConfig()
                    .AddServicesConfig()
                    .AddHandelValidationErrorConfig();


            return services;
        }
        private static IServiceCollection AddDbContextConfig(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("DefaultConnection String Not Found");
            services.AddDbContext<ApplicationDbContext>(option =>
            option.UseSqlServer(connectionString));
            return services;
        }
        private static IServiceCollection AddExceptionHandlerConfig(this IServiceCollection services)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>()
                    .AddProblemDetails();
            return services;
        }
        private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
        {
            // Add Mapster Global Configration
            var mappConfig = TypeAdapterConfig.GlobalSettings;
            mappConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddScoped<MappingConfiguration>();
            return services;
        }
        private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
        {
            // Add FluentValidation
            services.AddFluentValidationAutoValidation()
                    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
        private static IServiceCollection AddServicesConfig(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepositories<>), typeof(GenericRepositories<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            return services;
        }
        private static IServiceCollection AddHandelValidationErrorConfig(this IServiceCollection services)
        {
            //Add Validation Error
            services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                         .SelectMany(p => p.Value.Errors)
                                                         .Select(p => p.ErrorMessage).ToList();
                    var validationResponseError = Result.FailureList(
                                                             errors.Select(x =>
                                                             new Error(StatusCodes.Status409Conflict.ToString(), x, 404)
                                                         ).ToList());
                    
                    return new BadRequestObjectResult(validationResponseError.Errors);
                };
            });
            return services;
        }
    }
}