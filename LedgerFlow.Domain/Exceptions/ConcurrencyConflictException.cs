namespace LedgerFlow.Domain.Exceptions;

public class ConcurrencyConflictException : Exception
{
    public ConcurrencyConflictException(Guid streamId, int version) : base($"The LedgerEntry with the stream id {streamId} with version {version} encountered concurrency error") { }
}
