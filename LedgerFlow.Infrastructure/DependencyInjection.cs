using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Infrastructure.EventStore;
using LedgerFlow.Infrastructure.Messaging.Consumers;
using LedgerFlow.Infrastructure.Persistence;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IDbConnectionFactory, DbContextFactory>();
        services.AddScoped<IEventStore, SqlEventStore>();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<AssetDepositedConsumer>();
            x.AddConsumer<AssetWithdrawnConsumer>();
            x.AddConsumer<TradeExecutedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}
