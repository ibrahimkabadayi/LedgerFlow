namespace LedgerFlow.Infrastructure.Messaging.Contracts;

public record AssetDeposited(
    Guid WalletId,
    decimal Amount,
    string Currency,
    DateTime OccurredAt
    );
