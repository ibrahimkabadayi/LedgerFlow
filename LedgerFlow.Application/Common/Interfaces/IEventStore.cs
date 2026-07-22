using LedgerFlow.Domain.Events;

namespace LedgerFlow.Application.Common.Interfaces;

public interface IEventStore
{
    Task AppendAsync(Guid streamId, string streamType, int expectedVersion, IEnumerable<IDomainEvent> events);
    Task<IEnumerable<IDomainEvent>> GetStreamASync(Guid id);
}
