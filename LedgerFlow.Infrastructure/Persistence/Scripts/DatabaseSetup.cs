using Dapper;

namespace LedgerFlow.Infrastructure.Persistence.Scripts;

public class DatabaseSetup(IDbConnectionFactory connectionFactory)
{
    public async Task EnsureCreated()
    {
        using var connection = connectionFactory.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        var script = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Events')
            BEGIN
                CREATE TABLE Events (
                    Id BIGINT IDENTITY PRIMARY KEY,
                    StreamId UNIQUEIDENTIFIER NOT NULL,
                    StreamType NVARCHAR(100) NOT NULL,
                    Version INT NOT NULL,
                    EventType NVARCHAR(200) NOT NULL,
                    Payload NVARCHAR(MAX) NOT NULL,
                    OccurredAt DATETIME2 NOT NULL,
                    CONSTRAINT UQ_Stream_Version UNIQUE (StreamId, Version)
                );
                CREATE INDEX IX_Events_StreamId ON Events (StreamId, Version);
            END

            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'AccountBalances')
            BEGIN
                CREATE TABLE AccountBalances (
                    WalletId UNIQUEIDENTIFIER NOT NULL,
                    Currency NVARCHAR(20) NOT NULL,
                    Balance DECIMAL(18,8) NOT NULL,
                    LastUpdatedAt DATETIME2 NOT NULL,
                    PRIMARY KEY (WalletId, Currency)
                );
            END";

        await connection.ExecuteAsync(script, transaction);

        transaction.Commit();
    }
}