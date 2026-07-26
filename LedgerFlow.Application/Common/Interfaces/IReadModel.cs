using LedgerFlow.Application.Common.Models;
using LedgerFlow.Domain.ValueObjects;

namespace LedgerFlow.Application.Common.Interfaces;

public interface IReadModel
{
    Task<IEnumerable<AccountBalanceView>> GetAccountBalances(Guid walletId);
    Task UpdateAccountBalance(AccountBalanceView accountBalance);
}
