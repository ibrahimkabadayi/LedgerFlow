using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Application.Common.Models;
using MediatR;

namespace LedgerFlow.Application.Queries.GetAccountBalances;

public class GetAccountBalancesQueryHandler(IReadModel readModel) : IRequestHandler<GetAccountBalancesQuery, IEnumerable<AccountBalanceView>>
{
    public async Task<IEnumerable<AccountBalanceView>> Handle(GetAccountBalancesQuery request, CancellationToken cancellationToken)
    {
        return await readModel.GetAccountBalances(request.WalletId);
    }
}

