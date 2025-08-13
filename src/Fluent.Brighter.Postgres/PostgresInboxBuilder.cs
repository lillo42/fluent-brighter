using Paramore.Brighter;
using Paramore.Brighter.Inbox.Postgres;

namespace Fluent.Brighter.Postgres;

public sealed class PostgresInboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    public PostgresInboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmARelationalDbConnectionProvider? _connectionProvider;

    public PostgresInboxBuilder SetConnectionProvider(IAmARelationalDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    internal PostgreSqlInbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }
        
        return _connectionProvider == null ? new PostgreSqlInbox(_configuration) : new PostgreSqlInbox(_configuration, _connectionProvider);
    }
}