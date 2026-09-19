using LedgerFlow.Application.Commands;
using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Domain.Aggragates;
using MediatR;

namespace LedgerFlow.Application.Handlers;

public class RecordDepositHandler(IEventStore eventStore, IMediator mediator) : IRequestHandler<RecordDepositCommand>
{
    public async Task Handle(RecordDepositCommand request, CancellationToken cancellationToken)
    {
        var history = await eventStore.GetStreamAsync(request.WalletId);
        var entry = LedgerEntry.Replay(request.WalletId, history);
        var expectedVersion = entry.Version;

        entry.ApplyDeposit(request.Money);

        var eventsToPublish = entry.GetDomainEvents().ToList();
        await eventStore.AppendAsync(request.WalletId, nameof(LedgerEntry), expectedVersion, eventsToPublish);
        entry.ClearUncommitedEvents();

        foreach (var @event in eventsToPublish)
        {
            await mediator.Publish(@event, cancellationToken);
        }
    }
}
