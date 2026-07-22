using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace LedgerFlow.Infrastructure.Persistence;

public class DbContextFactory : IDbConnectionFactory
{
    private readonly string _connectionString = string.Empty;

    public DbContextFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("LedgerFlowDb")
            ?? throw new ArgumentException("Can not get the connection string.", nameof(configuration));
    }

    public IDbConnection CreateConnection()
    {
        var connection = new SqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
