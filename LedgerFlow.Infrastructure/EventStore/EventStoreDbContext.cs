using LedgerFlow.Infrastructure.Persistence;
using System.Data;

namespace LedgerFlow.Infrastructure.EventStore;

public class EventStoreDbContext : IDisposable
{
    private readonly IDbConnection _connection;
    private IDbTransaction? _transaction;

    public EventStoreDbContext(IDbConnectionFactory factory)
    {
        _connection = factory.CreateConnection();
    }

    public IDbTransaction BeginTransaction()
    {
        _transaction = _connection.BeginTransaction();
        return _transaction;
    }

    public void Commit()
    {
        _transaction?.Commit();
        _transaction?.Dispose();
        _transaction = null;
    }

    public void Rollback()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null;
    }

    public IDbTransaction? CurrentTransaction => _transaction;

    public void Dispose()
    {
        _connection?.Dispose();
        _transaction?.Dispose();
    }
}
