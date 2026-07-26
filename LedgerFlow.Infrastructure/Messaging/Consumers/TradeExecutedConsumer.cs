using LedgerFlow.Application.Commands;
using LedgerFlow.Infrastructure.Messaging.Contracts;
using MassTransit;
using MassTransit.Mediator;

namespace LedgerFlow.Infrastructure.Messaging.Consumers;

public class TradeExecutedConsumer(IMediator mediator) : IConsumer<TradeExucuted>
{
    public async Task Consume(ConsumeContext<TradeExucuted> context)
    {
        var message = context.Message;
        await mediator.Send(new RecordTradeCommand(
            message.WalletId,
            new Domain.ValueObjects.Money(message.BoughtAmount, message.BoughtCurrency),
            new Domain.ValueObjects.Money(message.SoldAmount, message.SoldCurrency)
            ));
    }
}
