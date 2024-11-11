using Core.Common.AppSettings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;
using static Infrastructure.Persistence.DbConnectionFactory;

namespace Infrastructure.Persistence;

internal interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(ConnectionType type);
}

internal class DbConnectionFactory(IOptions<ConnectionStrings> _connections) : IDbConnectionFactory
{
    private ConnectionStrings _connectionsValue => _connections.Value;

    public enum ConnectionType
    {
        SqlServer
    }

    private async Task<IDbConnection> CreateSqlServerConnectionAsync(ConnectionType type)
    {
        string connectionString = type switch
        {
            ConnectionType.SqlServer => _connectionsValue.SqlServer,
            _ => "",
        };

        var conn = new SqlConnection(connectionString);

        await conn.OpenAsync();

        return conn;
    }

    public async Task<IDbConnection> CreateConnectionAsync(ConnectionType type) => type switch
    {
        ConnectionType.SqlServer => await CreateSqlServerConnectionAsync(type),
        _ => throw new InvalidOperationException($"Invalid connection type: {type}")
    };
}