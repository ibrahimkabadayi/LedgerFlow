using LedgerFlow.Domain.ValueObjects;

namespace LedgerFlow.Domain.Events;

public record AssetDepositedRecorded(
    Guid LedgerEntryId,
    Money Money,
    DateTime OccurredAt)
    : IDomainEvent;

