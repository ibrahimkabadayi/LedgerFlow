using LedgerFlow.Domain.Events;

namespace LedgerFlow.Infrastructure.EventStore;

public class EventTypeRegistry
{
    private readonly Dictionary<string, Type> _map = new()
    {
        [nameof(LedgerEntryCreated)] = typeof(LedgerEntryCreated),
        [nameof(AssetDepositedRecorded)] = typeof(AssetDepositedRecorded),
        [nameof(AssetWithdrawnRecorded)] = typeof(AssetWithdrawnRecorded),
        [nameof(TradeRecorded)] = typeof(TradeRecorded),
    };

    public Type Resolve(string eventType) =>
        _map.TryGetValue(eventType, out var type)
            ? type
            : throw new InvalidOperationException($"Bilinmeyen event tipi: {eventType}");
}
