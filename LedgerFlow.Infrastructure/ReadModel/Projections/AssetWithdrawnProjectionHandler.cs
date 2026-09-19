using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Application.Common.Models;
using LedgerFlow.Domain.Events;
using MediatR;

namespace LedgerFlow.Infrastructure.ReadModel.Projections;

public class AssetWithdrawnProjectionHandler(IReadModel readModel) : INotificationHandler<AssetWithdrawnRecorded>
{
    public async Task Handle(AssetWithdrawnRecorded notification, CancellationToken cancellationToken)
    {
        var accountViews = await readModel.GetAccountBalances(notification.LedgerEntryId);
        var current = accountViews.FirstOrDefault(b => b.Currency.Equals(notification.Money.Currency, StringComparison.OrdinalIgnoreCase));

        if (current is not null && current.Balance >= notification.Money.Amount)
        {
            var updated = new AccountBalanceView
            {
                WalletId = notification.LedgerEntryId,
                Currency = notification.Money.Currency,
                Balance = current.Balance - notification.Money.Amount,
                LastUpdatedAt = notification.OccurredAt
            };

            await readModel.UpdateAccountBalance(updated);
        }
    }
}
