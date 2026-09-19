using Dapper;
using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Domain.Events;
using LedgerFlow.Domain.Exceptions;
using LedgerFlow.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;

namespace LedgerFlow.Infrastructure.EventStore;

public class SqlEventStore(IDbConnectionFactory dbConnection, EventSerializer serializer) : IEventStore
{
    public async Task AppendAsync(Guid streamId, string streamType, int expectedVersion, IEnumerable<IDomainEvent> events)
    {
        using var connection = dbConnection.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            var version = expectedVersion;

            foreach (var @event in events)
            {
                version++;

                var (eventType, payload) = serializer.Serialize(@event);

                const string query = @"
                    INSERT INTO Events (StreamId, StreamType, Version, EventType, Payload, OccurredAt)
                    VALUES (@StreamId, @StreamType, @Version, @EventType, @Payload, @OccurredAt)";

                await connection.ExecuteAsync(query, new
                {
                    StreamId = streamId,
                    StreamType = streamType,
                    Version = version,
                    EventType = eventType,
                    Payload = payload,
                    OccurredAt = DateTime.UtcNow
                }, transaction);
            }

            transaction.Commit();
        }
        catch (SqlException ex) when (ex.Number == 2627)
        {
            transaction.Rollback();
            throw new ConcurrencyConflictException(streamId, expectedVersion);
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<IEnumerable<IDomainEvent>> GetStreamAsync(Guid streamId)
    {
        using var connection = dbConnection.CreateConnection();
        connection.Open();

        var sql = @"SELECT StreamId, StreamType, Version, EventType, Payload, OccurredAt
            FROM Events
            WHERE StreamId = @StreamId
            ORDER BY Version ASC";

        var records = await connection.QueryAsync<EventRecord>(sql, new { StreamId = streamId });

        return records.Select(r => serializer.Deserialize(r.EventType, r.Payload));
    }
}
