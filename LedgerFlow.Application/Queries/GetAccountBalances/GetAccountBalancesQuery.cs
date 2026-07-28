using LedgerFlow.Application.Common.Models;
using MediatR;

namespace LedgerFlow.Application.Queries.GetAccountBalances;

public record GetAccountBalancesQuery(
    Guid WalletId
    ) : IRequest<IEnumerable<AccountBalanceView>>;
