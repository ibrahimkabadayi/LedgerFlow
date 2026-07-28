using LedgerFlow.Application.Common.Models;
using MediatR;

namespace LedgerFlow.Application.Commands;

public record GetAllAccountBalancesCommand(
    Guid WalletId
    ) : IRequest<List<AccountBalanceView>>;
