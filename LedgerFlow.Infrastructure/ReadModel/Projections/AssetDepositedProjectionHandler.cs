using LedgerFlow.Application.Commands;
using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Application.Common.Models;
using MassTransit;
using MediatR;

namespace LedgerFlow.Infrastructure.ReadModel.Projections;

public class AssetDepositedProjectionHandler(IReadModel readModel) : IRequestHandler<RecordDepositCommand>
{
    public async Task Handle(RecordDepositCommand request, CancellationToken cancellationToken)
    {
        var accountViews = await readModel.GetAccountBalances(request.WalletId);
        var current = accountViews.FirstOrDefault(b => b.Currency == request.Money.Currency);

        var updated = new AccountBalanceView
        {
            WalletId = request.WalletId,
            Currency = request.Money.Currency,
            Balance = (current?.Balance ?? 0) + request.Money.Amount,
            LastUpdatedAt = DateTime.UtcNow
        };

        await readModel.UpdateAccountBalance(updated);
    }
}
