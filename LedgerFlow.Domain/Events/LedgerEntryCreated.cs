namespace LedgerFlow.Domain.Events;

public record LedgerEntryCreated(
    Guid LedgerEntryId,
    DateTime OccurredAt
    ) : IDomainEvent;

