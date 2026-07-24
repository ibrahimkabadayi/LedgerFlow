using LedgerFlow.Infrastructure.Messaging.Contracts;
using MassTransit;

namespace LedgerFlow.Infrastructure.Messaging.Consumers;

public class AssetDepositedConsumer : IConsumer<AssetDeposited>
{
    public Task Consume(ConsumeContext<AssetDeposited> context)
    {
        throw new NotImplementedException();
    }
}
