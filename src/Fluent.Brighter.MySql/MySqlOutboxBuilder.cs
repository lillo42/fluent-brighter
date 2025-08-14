using Paramore.Brighter;
using Paramore.Brighter.Outbox.MySql;

namespace Fluent.Brighter.MySql;

public sealed class MySqlOutboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    public MySqlOutboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmARelationalDbConnectionProvider? _connectionProvider;

    public MySqlOutboxBuilder SetConnectionProvider(IAmARelationalDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    internal MySqlOutbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }
        
        return _connectionProvider == null ? new MySqlOutbox(_configuration) : new MySqlOutbox(_configuration, _connectionProvider);
    }
}