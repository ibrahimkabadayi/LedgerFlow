using LedgerFlow.Application.Common.Models;
using MediatR;

namespace LedgerFlow.Application.Commands;

public record GetAccountBalanceForOneCurrencyCommand(
    Guid WalletId,
    string Currency
    ) : IRequest<AccountBalanceView>;
