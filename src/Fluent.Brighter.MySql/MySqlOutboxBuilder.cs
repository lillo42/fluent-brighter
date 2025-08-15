using Paramore.Brighter;
using Paramore.Brighter.Outbox.MySql;

namespace Fluent.Brighter.MySql;

/// <summary>
/// A fluent builder used to configure and create a MySQL-based <see cref="MySqlOutbox"/> for message persistence.
/// </summary>
public sealed class MySqlOutboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    /// <summary>
    /// Sets the MySQL database configuration for the outbox
    /// </summary>
    /// <param name="configuration">The relational database configuration containing connection details</param>
    /// <returns>This builder for method chaining</returns>
    /// <remarks>
    /// This is the standard configuration method for most scenarios. 
    /// When used without <see cref="SetConnectionProvider"/>, the builder creates a default connection provider.
    /// </remarks>
    public MySqlOutboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmARelationalDbConnectionProvider? _connectionProvider;


    /// <summary>
    /// Sets a custom connection provider for the outbox
    /// </summary>
    /// <param name="connectionProvider">The connection provider implementation to use</param>
    /// <returns>This builder for method chaining</returns>
    /// <remarks>
    /// Use this to provide custom connection management (e.g., connection pooling or proxy handling). 
    /// Requires prior configuration via <see cref="SetConfiguration"/>.
    /// </remarks>
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