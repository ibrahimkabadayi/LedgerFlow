using LedgerFlow.Application.Commands;
using LedgerFlow.Domain.ValueObjects;
using MassTransit;
using MediatR;
using Shared.Messages;

namespace LedgerFlow.Infrastructure.Messaging.Consumers;

public class TradeExecutedConsumer(IMediator mediator) : IConsumer<TradeExecuted>
{
    public async Task Consume(ConsumeContext<TradeExecuted> context)
    {
        var message = context.Message;
        await mediator.Send(new RecordTradeCommand(
            message.WalletId,
            new Money(message.BoughtAmount, message.BoughtCurrency),
            new Money(message.SoldAmount, message.SoldCurrency)
        ));
    }
}
