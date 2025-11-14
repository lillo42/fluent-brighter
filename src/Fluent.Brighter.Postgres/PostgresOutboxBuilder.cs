using Npgsql;

using Paramore.Brighter;
using Paramore.Brighter.Outbox.PostgreSql;

namespace Fluent.Brighter.Postgres;

/// <summary>
/// Builder class for configuring and creating a PostgreSQL outbox.
/// The outbox pattern ensures reliable message publishing by storing outgoing messages in PostgreSQL
/// as part of the same transaction as domain changes, guaranteeing eventual consistency.
/// </summary>
public sealed class PostgresOutboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    /// <summary>
    /// Sets the database configuration for the PostgreSQL outbox.
    /// </summary>
    /// <param name="configuration">The relational database configuration containing connection details and settings.</param>
    /// <returns>The current <see cref="PostgresOutboxBuilder"/> instance for method chaining.</returns>
    public PostgresOutboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmARelationalDbConnectionProvider? _connectionProvider;

    /// <summary>
    /// Sets a custom database connection provider for the PostgreSQL outbox.
    /// Use this when you need to provide custom connection management logic.
    /// </summary>
    /// <param name="connectionProvider">The custom connection provider implementation.</param>
    /// <returns>The current <see cref="PostgresOutboxBuilder"/> instance for method chaining.</returns>
    public PostgresOutboxBuilder SetConnectionProvider(IAmARelationalDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    private NpgsqlDataSource? _dataSource;

    /// <summary>
    /// Sets an Npgsql data source for the PostgreSQL outbox.
    /// This provides an alternative way to configure database connections using Npgsql's data source API.
    /// </summary>
    /// <param name="dataSource">The Npgsql data source to use for database connections.</param>
    /// <returns>The current <see cref="PostgresOutboxBuilder"/> instance for method chaining.</returns>
    public PostgresOutboxBuilder SetDataSource(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
        return this;
    }
    
    /// <summary>
    /// Builds and returns a configured <see cref="PostgreSqlOutbox"/> instance.
    /// This method is called internally to create the outbox with the configured settings.
    /// The outbox will use either the connection provider or data source, depending on which was configured.
    /// </summary>
    /// <returns>A configured <see cref="PostgreSqlOutbox"/> instance.</returns>
    /// <exception cref="ConfigurationException">Thrown when no database configuration has been set via <see cref="SetConfiguration"/>.</exception>
    internal PostgreSqlOutbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }
        
        return _connectionProvider == null ? new PostgreSqlOutbox(_configuration, _dataSource) : new PostgreSqlOutbox(_configuration, _connectionProvider);
    }
}