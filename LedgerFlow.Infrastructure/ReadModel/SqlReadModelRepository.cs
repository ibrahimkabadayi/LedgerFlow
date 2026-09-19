using Dapper;
using LedgerFlow.Application.Common.Interfaces;
using LedgerFlow.Application.Common.Models;
using LedgerFlow.Infrastructure.Persistence;

namespace LedgerFlow.Infrastructure.ReadModel;

public class SqlReadModelRepository(IDbConnectionFactory connectionFactory) : IReadModel
{
    public async Task<IEnumerable<AccountBalanceView>> GetAccountBalances(Guid walletId)
    {
        using var connection = connectionFactory.CreateConnection();
        connection.Open();

        var sql = @"
                    SELECT WalletId, Currency, Balance, LastUpdatedAt 
                    FROM AccountBalances
                    WHERE WalletId = @WalletId;";

        return await connection.QueryAsync<AccountBalanceView>(sql, new { WalletId = walletId });
    }

    public async Task UpdateAccountBalance(AccountBalanceView accountBalance)
    {
        using var connection = connectionFactory.CreateConnection();
        connection.Open();

        var sql = @"
                    MERGE AccountBalances AS Target
                    USING (SELECT @WalletId AS WalletId, @Currency AS Currency) AS Source
                    ON Target.WalletId = Source.WalletId AND Target.Currency = Source.Currency
                    WHEN MATCHED THEN
                        UPDATE SET
                            Target.Balance = @Balance, Target.LastUpdatedAt = @LastUpdatedAt
                    WHEN NOT MATCHED THEN
                        INSERT (WalletId, Currency, Balance, LastUpdatedAt) 
                        VALUES (Source.WalletId, Source.Currency, @Balance, @LastUpdatedAt);";

        await connection.ExecuteAsync(sql, new
        {
            accountBalance.WalletId,
            accountBalance.Currency,
            accountBalance.Balance,
            accountBalance.LastUpdatedAt
        });
    }
}
