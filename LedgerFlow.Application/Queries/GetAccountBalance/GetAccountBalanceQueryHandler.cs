using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Application.Common.Models;
using MediatR;

namespace LedgerFlow.Application.Queries.GetAccountBalance;

public class GetAccountBalanceQueryHandler(IReadModel readModel) : IRequestHandler<GetAccountBalanceQuery, AccountBalanceView>
{
    public async Task<AccountBalanceView> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
    {
        var accountBalances = await readModel.GetAccountBalances(request.WalletId);
        var wantedCurrent = accountBalances.FirstOrDefault(x => x.Currency.Equals(request.Currency));

        if (wantedCurrent != null)
        {
            return wantedCurrent;
        }
        else
        {
            return new AccountBalanceView
            {
                WalletId = request.WalletId,
                Currency = request.Currency,
                Balance = 0,
                LastUpdatedAt = DateTime.UtcNow,
            };
        }
    }
}
