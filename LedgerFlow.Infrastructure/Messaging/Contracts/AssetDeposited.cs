namespace LedgerFlow.Infrastructure.Messaging.Contracts;

public record AssetDeposited(Guid walletId, decimal Amount, string Currency, DateTime OccurredAt);
