namespace LedgerFlow.Infrastructure.EventStore;

public record EventRecord(
    string EventType,
    string Payload
    );