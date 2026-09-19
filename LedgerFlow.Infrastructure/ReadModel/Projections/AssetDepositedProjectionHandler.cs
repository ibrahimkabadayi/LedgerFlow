using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Application.Common.Models;
using LedgerFlow.Domain.Events;
using MediatR;

namespace LedgerFlow.Infrastructure.ReadModel.Projections;

public class AssetDepositedProjectionHandler(IReadModel readModel) : INotificationHandler<AssetDepositedRecorded>
{
    public async Task Handle(AssetDepositedRecorded notification, CancellationToken cancellationToken)
    {
        var accountViews = await readModel.GetAccountBalances(notification.LedgerEntryId);
        var current = accountViews.FirstOrDefault(b => b.Currency.Equals(notification.Money.Currency, StringComparison.OrdinalIgnoreCase));

        var updated = new AccountBalanceView
        {
            WalletId = notification.LedgerEntryId,
            Currency = notification.Money.Currency,
            Balance = (current?.Balance ?? 0) + notification.Money.Amount,
            LastUpdatedAt = notification.OccurredAt
        };

        await readModel.UpdateAccountBalance(updated);
    }
}
