using LedgerFlow.Application.Commands;
using LedgerFlow.Domain.ValueObjects;
using MassTransit;
using MediatR;
using Shared.Messages;

namespace LedgerFlow.Infrastructure.Messaging.Consumers;

public class AssetWithdrawnConsumer(IMediator mediator) : IConsumer<AssetWithdrawn>
{
    public async Task Consume(ConsumeContext<AssetWithdrawn> context)
    {
        var message = context.Message;
        await mediator.Send(new RecordWithdrawnCommand(
            message.WalletId,
            new Money(message.Amount, message.Currency)
        ));
    }
}
