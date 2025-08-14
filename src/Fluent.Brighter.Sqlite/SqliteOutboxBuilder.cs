using Paramore.Brighter;
using Paramore.Brighter.Outbox.Sqlite;

namespace Fluent.Brighter.Sqlite;

public sealed class SqliteOutboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    public SqliteOutboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmARelationalDbConnectionProvider? _connectionProvider;

    public SqliteOutboxBuilder SetConnectionProvider(IAmARelationalDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    internal SqliteOutbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }
        
        return _connectionProvider == null ? new SqliteOutbox(_configuration) : new SqliteOutbox(_configuration, _connectionProvider);
    }
}