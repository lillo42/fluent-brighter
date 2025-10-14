using System;

using Paramore.Brighter;
using Paramore.Brighter.MsSql;
using Paramore.Brighter.Outbox.MsSql;

namespace Fluent.Brighter.SqlServer;

/// <summary>
/// Provides a fluent builder for configuring and creating an instance of <see cref="MsSqlOutbox"/>,
/// which implements the outbox pattern for reliable message publishing using Microsoft SQL Server.
/// </summary>
/// <remarks>
/// Extends the Paramore.Brighter framework with fluent configuration APIs and additional features.
/// The outbox pattern ensures that business data and outgoing messages are persisted atomically,
/// enabling eventual consistency and reliable message delivery.
/// </remarks>
public sealed class SqlServerOutboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    /// <summary>
    /// Sets the relational database configuration used to connect to and interact with the SQL Server database.
    /// </summary>
    /// <param name="configuration">The database configuration containing connection details and settings.</param>
    /// <returns>The current instance of <see cref="SqlServerOutboxBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
    public SqlServerOutboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        
        _configuration = configuration;
        return this;
    }

    private IAmARelationalDbConnectionProvider? _connectionProvider;

    /// <summary>
    /// Sets a custom connection provider for obtaining database connections.
    /// If not provided, a default <see cref="MsSqlConnectionProvider"/> will be used.
    /// </summary>
    /// <param name="connectionProvider">The connection provider implementation.</param>
    /// <returns>The current instance of <see cref="SqlServerOutboxBuilder"/> to allow method chaining.</returns>
    public SqlServerOutboxBuilder SetConnectionProvider(IAmARelationalDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    /// <summary>
    /// Builds and returns a configured instance of <see cref="MsSqlOutbox"/>.
    /// </summary>
    /// <returns>A fully configured <see cref="MsSqlOutbox"/> instance.</returns>
    /// <exception cref="ConfigurationException">Thrown if no configuration has been provided via <see cref="SetConfiguration"/>.</exception>
    internal MsSqlOutbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }

        _connectionProvider ??= new MsSqlConnectionProvider(_configuration);
        return new MsSqlOutbox(_configuration, _connectionProvider);
    }
}