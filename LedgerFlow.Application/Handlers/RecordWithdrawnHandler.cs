using LedgerFlow.Application.Commands;
using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Domain.Aggragates;
using MediatR;

namespace LedgerFlow.Application.Handlers;

public class RecordWithdrawnHandler(IEventStore eventStore) : IRequestHandler<RecordWithdrawnCommand>
{
    public async Task Handle(RecordWithdrawnCommand request, CancellationToken cancellationToken)
    {
        var history = await eventStore.GetStreamAsync(request.WalletId);
        var entry = LedgerEntry.Replay(history);
        var expectedVersion = entry.Version;

        entry.ApplyWithdraw(request.Money);

        await eventStore.AppendAsync(entry.Id, nameof(entry), expectedVersion, entry.GetDomainEvents());

        entry.ClearUncommitedEvents();
    }
}
