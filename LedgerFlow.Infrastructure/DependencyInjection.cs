using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Infrastructure.EventStore;
using LedgerFlow.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IDbConnectionFactory, DbContextFactory>();
        services.AddScoped<IEventStore, SqlEventStore>();

        return services;
    }
}
