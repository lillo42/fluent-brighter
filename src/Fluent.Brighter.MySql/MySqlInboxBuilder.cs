using Paramore.Brighter;
using Paramore.Brighter.Inbox.MySql;

namespace Fluent.Brighter.MySql;

/// <summary>
/// A fluent builder used to configure and create a MySQL-based <see cref="MySqlInbox"/> for message persistence.
/// </summary>
/// <remarks>
/// This builder follows the fluent design pattern to configure either:
/// <list type="bullet">
/// <item><description>A database configuration for automatic connection management, or</description></item>
/// <item><description>A custom connection provider for advanced scenarios</description></item>
/// </list>
/// </remarks>
public sealed class MySqlInboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    /// <summary>
    /// Sets the MySQL database configuration for the inbox
    /// </summary>
    /// <param name="configuration">The relational database configuration</param>
    /// <returns>This builder for fluent chaining</returns>
    /// <remarks>
    /// Use this to configure connection strings and other database settings.
    /// When using this method, the builder will create its own connection provider.
    /// </remarks>
    public MySqlInboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmARelationalDbConnectionProvider? _connectionProvider;

    /// <summary>
    /// Sets a custom connection provider for the inbox
    /// </summary>
    /// <param name="connectionProvider">The connection provider to use</param>
    /// <returns>This builder for fluent chaining</returns>
    /// <remarks>
    /// Use this to provide a custom connection management implementation.
    /// When using this method, you don't need to call <see cref="SetConfiguration"/>.
    /// </remarks>
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