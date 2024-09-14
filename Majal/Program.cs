
using Majal.Api.Mapping;
using Majal.Repository.Persistence;
using Mapster;
using Microsoft.Identity.Client;
using System.Reflection;

namespace Majal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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

            app.UseAuthorization();

            
            app.MapControllers();

            app.UseExceptionHandler();
            app.Run();
        }
    }
}
