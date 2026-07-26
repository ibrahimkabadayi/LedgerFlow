using LedgerFlow.Application.Commands;
using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Domain.Aggragates;
using MediatR;

namespace LedgerFlow.Application.Handlers;

public class RecordTradeHandler(IEventStore eventStore) : IRequestHandler<RecordTradeCommand>
{
    public async Task Handle(RecordTradeCommand request, CancellationToken cancellationToken)
    {
        var history = await eventStore.GetStreamAsync(request.WalletId);
        var entry = LedgerEntry.Replay(history);
        var expectedVersion = entry.Version;

        entry.ApplyTrade(request.Bought, request.Sold);

        await eventStore.AppendAsync(entry.Id, nameof(entry), expectedVersion, entry.GetDomainEvents());

        entry.ClearUncommitedEvents();
    }
}
