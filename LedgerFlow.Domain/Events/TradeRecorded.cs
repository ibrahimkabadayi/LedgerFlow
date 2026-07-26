using LedgerFlow.Domain.ValueObjects;

namespace LedgerFlow.Domain.Events;

public record TradeRecorded(
    Guid LedgerEntryId,
    Money Bought,
    Money Sold,
    DateTime OccurredAt
    ) : IDomainEvent;