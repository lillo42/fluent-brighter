using Paramore.Brighter;

using Paramore.Brighter.Outbox.Sqlite;

namespace Fluent.Brighter.Sqlite;

/// <summary>
/// A fluent builder used to configure and create a SQLite outbox
/// </summary>
public sealed class SqliteOutboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    /// <summary>
    /// Sets the SQLite database configuration
    /// </summary>
    /// <param name="configuration">The configuration for connecting to the SQLite database</param>
    /// <returns>This builder for method chaining</returns>
    public SqliteOutboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
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
    public SqliteOutboxBuilder SetConnectionProvider(IAmARelationalDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    /// <summary>
    /// Constructs the SQLite outbox configuration
    /// </summary>
    /// <returns>A configured SqliteOutbox</returns>
    /// <exception cref="ConfigurationException">Thrown if database configuration is not provided</exception>
    internal SqliteOutbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("SQLite outbox configuration is missing. " +
                                             "You must provide database configuration using SetConfiguration() before building. ");
        }
        
        return _connectionProvider == null 
            ? new SqliteOutbox(_configuration) 
            : new SqliteOutbox(_configuration, _connectionProvider);
    }
}