namespace LedgerFlow.Infrastructure.Messaging.Contracts;

public record TradeExucuted(Guid WalletId, decimal Amount, string Currency, DateTime OccurredAt);
