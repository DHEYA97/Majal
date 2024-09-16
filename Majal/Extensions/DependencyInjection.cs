using FluentValidation.AspNetCore;
using Majal.Core.Interfaces.Repositories;
using Majal.Core.Interfaces.Service;
using Majal.Core.UnitOfWork;
using Majal.Repository.Persistence;
using Majal.Repository.Repositories;
using Majal.Repository.UnitOfWork;
using System.Reflection;
using Majal.Core.Abstractions;
using Majal.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Majal.Core.Interfaces.Authentication;
using Majal.Service.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Mapster;
using Majal.Core.Settinges;
using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Majal.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();


            services.AddDbContextConfig(configuration)
                    .AddAuthConfig(configuration)
                    .AddCorsConfig(configuration)
                    .AddMailConfig(configuration)
                    .AddHangFireConfig(configuration);

            services.AddSwaggerGen()
                    .AddEndpointsApiExplorer()
                    .AddExceptionHandlerConfig()
                    .AddMapsterConfig()
                    .AddFluentValidationConfig()
                    .AddServicesConfig()
                    .AddHandelValidationErrorConfig()
                    .AddHttpContextAccessorConfig();
            

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
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEmailSender, EmailService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
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

        private static IServiceCollection AddIdentityConfig(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(Options =>
            {
                Options.Password.RequiredLength = 8;
                Options.SignIn.RequireConfirmedEmail = true;
                Options.User.RequireUniqueEmail = true;
            });
            return services;
        }
        private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddSingleton<IJwtProvider, JwtProvider>();

            //Set Configure og JwtOptions from Appsetting

            services.AddOptions<JwtOptions>()
                    .BindConfiguration(JwtOptions.SectionName)
                    .ValidateDataAnnotations()
                    .ValidateOnStart();

            //Read into JwtOptions from Appsetting
            var jwtOption = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOption!.Key)),
                    ValidIssuer = jwtOption!.Issuer,
                    ValidAudience = jwtOption!.Audience
                };
            });


            //Add Idintity configration
            services.AddIdentityConfig();
            return services;
        }
        private static IServiceCollection AddCorsConfig(this IServiceCollection services, IConfiguration configuration)
        {
           // var allowOrigin = configuration.GetSection("AllowOrigin").Get<string[]>();


            services.AddCors(option => {
                option.AddDefaultPolicy(bulder =>
                                        bulder.AllowAnyOrigin()
                                              .AllowAnyMethod()
                                              .AllowAnyHeader()

                );
            }
                             );
            return services;
        }
        private static IServiceCollection AddMailConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSetting>(configuration.GetSection(MailSetting.SectionName));
            return services;
        }
        private static IServiceCollection AddHangFireConfig(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddHangfire(config => config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

            services.AddHangfireServer();
            return services;
        }
        private static IServiceCollection AddHttpContextAccessorConfig(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            return services;
        }
    }
}