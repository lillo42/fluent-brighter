using Paramore.Brighter;
using Paramore.Brighter.Inbox.MySql;

namespace Fluent.Brighter.MySql;

public sealed class MySqlInboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    public MySqlInboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmARelationalDbConnectionProvider? _connectionProvider;

    public MySqlInboxBuilder SetConnectionProvider(IAmARelationalDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    internal MySqlInbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }
        
        return _connectionProvider == null ? new MySqlInbox(_configuration) : new MySqlInbox(_configuration, _connectionProvider);
    }
}