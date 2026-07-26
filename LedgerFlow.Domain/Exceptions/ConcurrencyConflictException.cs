namespace LedgerFlow.Domain.Exceptions;

public class ConcurrencyConflictException(Guid streamId, int version) : Exception($"The LedgerEntry with the stream id {streamId} with version {version} encountered concurrency error")
{
}
