using Paramore.Brighter;
using Paramore.Brighter.Inbox.Sqlite;

namespace Fluent.Brighter.Sqlite;

/// <summary>
/// A fluent builder used to configure and create a SQLite inbox.
/// </summary>
public sealed class SqliteInboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    /// <summary>
    /// Sets the SQLite database configuration
    /// </summary>
    /// <param name="configuration">The configuration for connecting to the SQLite database</param>
    /// <returns>This builder for method chaining</returns>
    public SqliteInboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmARelationalDbConnectionProvider? _connectionProvider;

    /// <summary>
    /// Sets a custom connection provider (optional). If not provided, uses the default SQLite connection provider.
    /// </summary>
    /// <param name="connectionProvider">The connection provider to use for SQLite connections</param>
    /// <returns>This builder for method chaining</returns>
    public SqliteInboxBuilder SetConnectionProvider(IAmARelationalDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    /// <summary>
    /// Constructs the SQLite inbox configuration
    /// </summary>
    /// <returns>A configured SqliteInbox</returns>
    /// <exception cref="ConfigurationException">Thrown if database configuration is not provided</exception>
    internal SqliteInbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("SQLite inbox configuration is missing. " +
                                             "You must provide database configuration using SetConfiguration() before building. ");
        }
        
        return _connectionProvider == null 
            ? new SqliteInbox(_configuration) 
            : new SqliteInbox(_configuration, _connectionProvider);
    }
}