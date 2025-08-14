using Npgsql;

using Paramore.Brighter;
using Paramore.Brighter.Outbox.PostgreSql;

namespace Fluent.Brighter.Postgres;

public sealed class PostgresOutboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    public PostgresOutboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmARelationalDbConnectionProvider? _connectionProvider;

    public PostgresOutboxBuilder SetConnectionProvider(IAmARelationalDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    private NpgsqlDataSource? _dataSource;

    public PostgresOutboxBuilder SetDataSource(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
        return this;
    }
    
    internal PostgreSqlOutbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }
        
        return _connectionProvider == null ? new PostgreSqlOutbox(_configuration, _dataSource) : new PostgreSqlOutbox(_configuration, _connectionProvider);
    }
}