using System.Data;

namespace LedgerFlow.Infrastructure.Persistence;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
