using Paramore.Brighter;
using Paramore.Brighter.Inbox.Sqlite;

namespace Fluent.Brighter.Sqlite;

public sealed class SqliteInboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    public SqliteInboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmARelationalDbConnectionProvider? _connectionProvider;

    public SqliteInboxBuilder SetConnectionProvider(IAmARelationalDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    internal SqliteInbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }
        
        return _connectionProvider == null ? new SqliteInbox(_configuration) : new SqliteInbox(_configuration, _connectionProvider);
    }
}