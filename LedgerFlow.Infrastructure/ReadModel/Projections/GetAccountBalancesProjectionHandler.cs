using LedgerFlow.Application.Commands;
using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Application.Common.Models;
using MediatR;

namespace LedgerFlow.Infrastructure.ReadModel.Projections;

public class GetAccountBalancesProjectionHandler(IReadModel readModel) : IRequestHandler<GetAllAccountBalancesCommand, IEnumerable<AccountBalanceView>>
{
    public async Task<IEnumerable<AccountBalanceView>> Handle(GetAllAccountBalancesCommand request, CancellationToken cancellationToken)
    {
        return await readModel.GetAccountBalances(request.WalletId);
    }
}

