using MassTransit;

namespace LedgerFlow.Infrastructure.Messaging.Consumers;

public class AssetWithdrawnConsumer : IConsumer<AssetWithdrawnConsumer>
{
    public Task Consume(ConsumeContext<AssetWithdrawnConsumer> context)
    {
        throw new NotImplementedException();
    }
}
