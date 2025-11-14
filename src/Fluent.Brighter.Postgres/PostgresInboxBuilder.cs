using Paramore.Brighter;
using Paramore.Brighter.Inbox.Postgres;

namespace Fluent.Brighter.Postgres;

/// <summary>
/// Builder class for configuring and creating a PostgreSQL inbox.
/// The inbox pattern ensures message deduplication and idempotent message processing by storing
/// information about received messages in PostgreSQL.
/// </summary>
public sealed class PostgresInboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    /// <summary>
    /// Sets the database configuration for the PostgreSQL inbox.
    /// </summary>
    /// <param name="configuration">The relational database configuration containing connection details and settings.</param>
    /// <returns>The current <see cref="PostgresInboxBuilder"/> instance for method chaining.</returns>
    public PostgresInboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmARelationalDbConnectionProvider? _connectionProvider;

    /// <summary>
    /// Sets a custom database connection provider for the PostgreSQL inbox.
    /// Use this when you need to provide custom connection management logic.
    /// </summary>
    /// <param name="connectionProvider">The custom connection provider implementation.</param>
    /// <returns>The current <see cref="PostgresInboxBuilder"/> instance for method chaining.</returns>
    public PostgresInboxBuilder SetConnectionProvider(IAmARelationalDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    /// <summary>
    /// Builds and returns a configured <see cref="PostgreSqlInbox"/> instance.
    /// This method is called internally to create the inbox with the configured settings.
    /// </summary>
    /// <returns>A configured <see cref="PostgreSqlInbox"/> instance.</returns>
    /// <exception cref="ConfigurationException">Thrown when no database configuration has been set via <see cref="SetConfiguration"/>.</exception>
    internal PostgreSqlInbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }
        
        return _connectionProvider == null ? new PostgreSqlInbox(_configuration) : new PostgreSqlInbox(_configuration, _connectionProvider);
    }
}