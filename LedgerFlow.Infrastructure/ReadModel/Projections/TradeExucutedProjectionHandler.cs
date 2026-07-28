using LedgerFlow.Application.Commands;
using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Application.Common.Models;
using MediatR;

namespace LedgerFlow.Infrastructure.ReadModel.Projections;

public class TradeExucutedProjectionHandler(IReadModel readModel) : IRequestHandler<RecordTradeCommand>
{
    public async Task Handle(RecordTradeCommand request, CancellationToken cancellationToken)
    {
        var balances = await readModel.GetAccountBalances(request.WalletId);

        var sold = balances.FirstOrDefault(x => x.Currency.Equals(request.Sold.Currency));
        var bought = balances.FirstOrDefault(x => x.Currency.Equals(request.Bought.Currency));

        if (sold is null)
        {
            throw new ArgumentException($"There is no account balance in {request.Sold.Currency} type.");
        }

        AccountBalanceView updateSold;

        if (sold.Balance >= request.Sold.Amount)
        {
            updateSold = new AccountBalanceView
            {
                WalletId = request.WalletId,
                Currency = request.Sold.Currency,
                Balance = sold.Balance - request.Sold.Amount,
                LastUpdatedAt = DateTime.UtcNow
            };
        } 
        else
        {
            throw new ArgumentException($"Sold currency balance must be bigger than {request.Sold.Amount}");
        }

        var updateBought = new AccountBalanceView
        {
            WalletId = request.WalletId,
            Currency = request.Bought.Currency,
            Balance = (bought?.Balance ?? 0) + request.Bought.Amount,
            LastUpdatedAt = DateTime.UtcNow
        };

        await readModel.UpdateAccountBalance(updateSold);
        await readModel.UpdateAccountBalance(updateBought);
    }
}
