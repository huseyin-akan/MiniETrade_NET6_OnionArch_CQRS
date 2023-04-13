using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniETrade.Application.Abstractions;
using MiniETrade.Application.Abstractions.MessageQue;
using MiniETrade.Application.Abstractions.Storage;
using MiniETrade.Application.Abstractions.Token;
using MiniETrade.Infrastructure.Enums;
using MiniETrade.Infrastructure.Services.MessageQue.MassTransit;
using MiniETrade.Infrastructure.Services.MessageQue.RabbitMQ;
using MiniETrade.Infrastructure.Services.Storage;
using MiniETrade.Infrastructure.Services.Storage.Local;
using MiniETrade.Infrastructure.Services.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniETrade.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IStorageService, StorageService>();
            serviceCollection.AddScoped<ITokenHandler, TokenHandler>();
            serviceCollection.AddScoped<IMQService, RabbitMQService>();
            serviceCollection.AddScoped<IMassTransitService, MassTransitService>();
        }

        //MassTransit Configuration
        public static void AddMassTransitRegistration(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddMassTransit(
                config =>
                {
                    config.UsingRabbitMq((context, _config) =>
                    {
                        _config.Host(configuration["RabbitMQ:Uri"], h =>
                        {
                            h.Username(configuration["RabbitMQ:Username"]);
                            h.Password(configuration["RabbitMQ:Password"]);
                        });
                    });
                });
        }

        public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : Storage, IStorage
        {
            serviceCollection.AddScoped<IStorage, T>();
        }
        public static void AddStorage(this IServiceCollection serviceCollection, StorageType storageType)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
                case StorageType.Azure:
                    //serviceCollection.AddScoped<IStorage, AzureStorage>();
                    break;
                case StorageType.AWS:

                    break;
                default:
                    serviceCollection.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }
    }
}
