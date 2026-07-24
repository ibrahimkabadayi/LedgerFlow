namespace LedgerFlow.Infrastructure.Messaging.Contracts;

public record AssetWithdrawn(Guid WalletId, decimal Amount, string Currency, DateTime OccurredAt);
