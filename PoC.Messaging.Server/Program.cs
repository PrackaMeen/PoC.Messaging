using PoC.Messaging.Server.Controllers;
using PoC.Messaging.Server.Hubs;
using PoC.Messaging.Server.Models;
using PoC.Repositories.StorageAccounts;

namespace PoC.Messaging.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();

            builder.Services.AddScoped<IQueueConnector, QueueConnector>();
            builder.Services.AddScoped<ITableConnector<User>, TableConnector<User>>();

            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<QueueMessage, QueueMessageDTO>();
            });

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
            app.MapHub<ChatHub>("/hub");

            app.UseCors((settings) =>
            {
                settings.AllowCredentials();
                settings.AllowAnyHeader();
                settings.AllowAnyMethod();
                settings.SetIsOriginAllowed((_) =>
                {
                    return true;
                });
            });

            app.Run();
        }
    }
}
