using LedgerFlow.Application.Commands;
using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Infrastructure.EventStore;
using LedgerFlow.Infrastructure.Messaging.Consumers;
using LedgerFlow.Infrastructure.Persistence;
using LedgerFlow.Infrastructure.Persistence.Scripts;
using LedgerFlow.Infrastructure.ReadModel;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IDbConnectionFactory, DbContextFactory>();
        services.AddScoped<IEventStore, SqlEventStore>();
        services.AddScoped<IReadModel, SqlReadModelRepository>();
        services.AddSingleton<EventTypeRegistry>();
        services.AddSingleton<EventSerializer>();
        services.AddScoped<DatabaseSetup>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(RecordDepositCommand).Assembly,
                typeof(DependencyInjection).Assembly
            );
        });

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
