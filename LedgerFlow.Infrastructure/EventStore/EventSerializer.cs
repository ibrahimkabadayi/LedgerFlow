using LedgerFlow.Domain.Events;
using System.Text.Json;

namespace LedgerFlow.Infrastructure.EventStore;

public class EventSerializer(EventTypeRegistry eventTypeRegistry)
{
    public (string EventType, string Payload) Serialize(IDomainEvent @event)
    {
        var eventType = @event.GetType().Name;
        var payload = JsonSerializer.Serialize(@event, eventType.GetType());
        return (eventType, payload);
    }

    public IDomainEvent Deserialize(string eventType, string payload)
    {
        var type = eventTypeRegistry.Resolve(eventType);
        return JsonSerializer.Deserialize(payload, type) as IDomainEvent ?? 
            throw new ArgumentException(nameof(type), "Could not deserialize to IDomainEvent");
    }
}
