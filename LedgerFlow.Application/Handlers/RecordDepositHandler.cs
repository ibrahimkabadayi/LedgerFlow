using LedgerFlow.Application.Commands;
using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Domain.Aggragates;
using MediatR;

namespace LedgerFlow.Application.Handlers;

public class RecordDepositHandler(IEventStore eventStore) : IRequestHandler<RecordDepositCommand>
{
    public async Task Handle(RecordDepositCommand request, CancellationToken cancellationToken)
    {
        var history = await eventStore.GetStreamAsync(request.WalletId);
        var entry = LedgerEntry.Replay(history);

        entry.ApplyDeposit(request.Money);

        await eventStore.AppendAsync(entry.Id, nameof(LedgerEntry), entry.Version - entry.GetDomainEvents().Count, entry.GetDomainEvents());

        entry.ClearUncommitedEvents();
    }
}
