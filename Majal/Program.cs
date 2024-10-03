using Hangfire;
using HangfireBasicAuthenticationFilter;
using Serilog;

namespace Majal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, configration) =>
            {
                configration.ReadFrom.Configuration(context.Configuration);
            });

            builder.Services.AddDependency(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute("/ErroresNotFound/{0}");
            app.UseHttpsRedirection();
            app.UseCors();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization(); 
            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                Authorization =
                [
                    new HangfireCustomBasicAuthenticationFilter
                    {
                        User = app.Configuration.GetValue<string>("HangfireSettings:Username"),
                        Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
                    }
                ],
                DashboardTitle = "Majal Job Dashboard",
            });
            app.MapControllers();
            app.UseExceptionHandler();
            app.Run();
        }
    }
}
