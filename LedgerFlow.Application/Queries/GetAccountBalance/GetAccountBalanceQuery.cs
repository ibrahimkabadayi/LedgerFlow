using LedgerFlow.Application.Common.Models;
using MediatR;

namespace LedgerFlow.Application.Queries.GetAccountBalance;

public record GetAccountBalanceQuery(
    Guid WalletId,
    string Currency
    ) : IRequest<AccountBalanceView>;
