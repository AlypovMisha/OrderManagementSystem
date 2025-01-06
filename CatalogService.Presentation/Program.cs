using CatalogService.Application.Interfaces;
using CatalogService.Application.Services;
using CatalogService.Application.Validations;
using CatalogService.Infrastructure.Data;
using CatalogService.Infrastructure.Repositories;
using Serilog;
using FluentValidation;

namespace CatalogService.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(@"C:\Users\Миша\Desktop\Полигон\logs\log.txt")
                .CreateLogger();

            Log.Information("Starting the application.");
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddScoped <ICatalogRepository,CatalogRepository>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();
            builder.Host.UseSerilog();
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

        }
    }
}
