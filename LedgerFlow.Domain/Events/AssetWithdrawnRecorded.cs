using LedgerFlow.Domain.ValueObjects;

namespace LedgerFlow.Domain.Events;

public record AssetWithdrawnRecorded(
    Guid LedgerEntryId,
    Money Money,
    DateTime OccurredAt) 
    : IDomainEvent;
