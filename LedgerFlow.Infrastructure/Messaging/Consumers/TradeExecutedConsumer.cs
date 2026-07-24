using LedgerFlow.Infrastructure.Messaging.Contracts;
using MassTransit;

namespace LedgerFlow.Infrastructure.Messaging.Consumers;

public class TradeExecutedConsumer : IConsumer<TradeExucuted>
{
    public Task Consume(ConsumeContext<TradeExucuted> context)
    {
        throw new NotImplementedException();
    }
}
