using LedgerFlow.Application.Commands;
using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Domain.Aggragates;
using MediatR;

namespace LedgerFlow.Application.Handlers;

public class RecordTradeHandler(IEventStore eventStore, IMediator mediator) : IRequestHandler<RecordTradeCommand>
{
    public async Task Handle(RecordTradeCommand request, CancellationToken cancellationToken)
    {
        var history = await eventStore.GetStreamAsync(request.WalletId);
        var entry = LedgerEntry.Replay(request.WalletId, history);
        var expectedVersion = entry.Version;

        entry.ApplyTrade(request.Bought, request.Sold);

        var eventsToPublish = entry.GetDomainEvents().ToList();
        await eventStore.AppendAsync(request.WalletId, nameof(LedgerEntry), expectedVersion, eventsToPublish);
        entry.ClearUncommitedEvents();

        foreach (var @event in eventsToPublish)
        {
            await mediator.Publish(@event, cancellationToken);
        }
    }
}
