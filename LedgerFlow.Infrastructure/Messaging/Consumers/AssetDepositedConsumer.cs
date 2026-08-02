using LedgerFlow.Infrastructure.Messaging.Contracts;
using LedgerFlow.Application.Commands;
using MassTransit;
using MassTransit.Mediator;

namespace LedgerFlow.Infrastructure.Messaging.Consumers;

public class AssetDepositedConsumer(IMediator mediator) : IConsumer<AssetDeposited>
{
    public async Task Consume(ConsumeContext<AssetDeposited> context)
    {
        var message = context.Message;
        await mediator.Send(new RecordDepositCommand(
            message.WalletId,
            new Domain.ValueObjects.Money(message.Amount, message.Currency)
            ));
    }
}