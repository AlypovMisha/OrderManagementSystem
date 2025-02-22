using CatalogService.Application.Extensions;
using CatalogService.Infrastructure;
using CatalogService.Infrastructure.ConfigurationDB;
using CatalogService.Presentation.Middleware;
using Serilog;

namespace CatalogService.Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(@"C:\Users\Миша\Desktop\Полигон\logs\log.txt")
                .CreateLogger();

            Log.Information("Starting the application.");
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Host.UseSerilog();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddAsyncInitializer<DbInitializer>();




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandlerMiddleware();

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            await app.InitAndRunAsync();
        }
    }
}
