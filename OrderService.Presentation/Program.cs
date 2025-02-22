using OrderService.Application.DTOs;
using OrderService.Application.Extensions;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure;
using OrderService.Infrastructure.Extensions;
using OrderService.Infrastructure.HttpClients;

namespace OrderService.Presentation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddHttpClient<ICatalogServiceClient, CatalogServiceClient>();
            builder.Services.AddApplicationServices();
            builder.Services.AddAsyncInitializer<DbInitializer>();
            builder.Services.AddProducer<OrderDTO>(builder.Configuration.GetSection("Kafka:Order"));

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

            await app.InitAndRunAsync();
        }
    }
}
