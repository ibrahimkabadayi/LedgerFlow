using LedgerFlow.Application.Commands;
using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Application.Common.Models;
using MediatR;

namespace LedgerFlow.Infrastructure.ReadModel.Projections;

public class AssetWithdrawnProjectionHandler(IReadModel readModel) : IRequestHandler<RecordWithdrawnCommand>
{
    public async Task Handle(RecordWithdrawnCommand request, CancellationToken cancellationToken)
    {
        var accountViews = await readModel.GetAccountBalances(request.WalletId);
        var current = accountViews.FirstOrDefault(b => b.Currency == request.Money.Currency);

        AccountBalanceView updated;

        if (current is not null && current?.Balance >= request.Money.Amount)
        {
            updated = new AccountBalanceView
            {
                WalletId = request.WalletId,
                Currency = request.Money.Currency,
                Balance = current.Balance - request.Money.Amount,
                LastUpdatedAt = DateTime.UtcNow
            };

            await readModel.UpdateAccountBalance(updated);
        }
    }
}
