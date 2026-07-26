namespace LedgerFlow.Infrastructure.Messaging.Contracts;

public record TradeExucuted(
    Guid WalletId,
    decimal BoughtAmount,
    string BoughtCurrency,
    decimal SoldAmount,
    string SoldCurrency,
    DateTime OccurredAt
    );
