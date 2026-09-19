using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Application.Common.Models;
using LedgerFlow.Domain.Events;
using MediatR;

namespace LedgerFlow.Infrastructure.ReadModel.Projections;

public class TradeExecutedProjectionHandler(IReadModel readModel) : INotificationHandler<TradeRecorded>
{
    public async Task Handle(TradeRecorded notification, CancellationToken cancellationToken)
    {
        var balances = await readModel.GetAccountBalances(notification.LedgerEntryId);

        var sold = balances.FirstOrDefault(x => x.Currency.Equals(notification.Sold.Currency, StringComparison.OrdinalIgnoreCase));
        var bought = balances.FirstOrDefault(x => x.Currency.Equals(notification.Bought.Currency, StringComparison.OrdinalIgnoreCase));

        if (sold is null)
        {
            throw new InvalidOperationException($"There is no account balance in {notification.Sold.Currency} type.");
        }

        if (sold.Balance < notification.Sold.Amount)
        {
            throw new InvalidOperationException($"Sold currency balance must be bigger than {notification.Sold.Amount}");
        }

        var updateSold = new AccountBalanceView
        {
            WalletId = notification.LedgerEntryId,
            Currency = notification.Sold.Currency,
            Balance = sold.Balance - notification.Sold.Amount,
            LastUpdatedAt = notification.OccurredAt
        };

        var updateBought = new AccountBalanceView
        {
            WalletId = notification.LedgerEntryId,
            Currency = notification.Bought.Currency,
            Balance = (bought?.Balance ?? 0) + notification.Bought.Amount,
            LastUpdatedAt = notification.OccurredAt
        };

        await readModel.UpdateAccountBalance(updateSold);
        await readModel.UpdateAccountBalance(updateBought);
    }
}
