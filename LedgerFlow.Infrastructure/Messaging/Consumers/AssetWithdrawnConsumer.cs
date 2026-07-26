using LedgerFlow.Application.Commands;
using LedgerFlow.Infrastructure.Messaging.Contracts;
using MassTransit;
using MassTransit.Mediator;

namespace LedgerFlow.Infrastructure.Messaging.Consumers;

public class AssetWithdrawnConsumer(IMediator mediator) : IConsumer<AssetWithdrawn>
{
    public async Task Consume(ConsumeContext<AssetWithdrawn> context)
    {
        var message = context.Message;
        await mediator.Send(new RecordWithdrawnCommand(
            message.WalletId,
            new Domain.ValueObjects.Money(message.Amount, message.Currency)
            ));
    }
}
