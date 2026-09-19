using LedgerFlow.Application.Commands;
using LedgerFlow.Domain.ValueObjects;
using MassTransit;
using MediatR;
using Shared.Messages;

namespace LedgerFlow.Infrastructure.Messaging.Consumers;

public class AssetDepositedConsumer(IMediator mediator) : IConsumer<AssetDeposited>
{
    public async Task Consume(ConsumeContext<AssetDeposited> context)
    {
        var message = context.Message;
        await mediator.Send(new RecordDepositCommand(
            message.WalletId,
            new Money(message.Amount, message.Currency)
        ));
    }
}