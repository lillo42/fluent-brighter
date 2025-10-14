using Paramore.Brighter;
using Paramore.Brighter.Inbox.MsSql;
using Paramore.Brighter.MsSql;

namespace Fluent.Brighter.SqlServer;

/// <summary>
/// Provides a fluent builder for configuring and creating an instance of <see cref="MsSqlInbox"/>,
/// which implements an inbox pattern for message deduplication using Microsoft SQL Server.
/// </summary>
public sealed class SqlServerInboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    /// <summary>
    /// Sets the relational database configuration used to connect to and interact with the SQL Server database.
    /// </summary>
    /// <param name="configuration">The database configuration containing connection details and settings.</param>
    /// <returns>The current instance of <see cref="SqlServerInboxBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
    public SqlServerInboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmARelationalDbConnectionProvider? _connectionProvider;

    /// <summary>
    /// Sets a custom connection provider for obtaining database connections.
    /// If not provided, a default <see cref="MsSqlConnectionProvider"/> will be used.
    /// </summary>
    /// <param name="connectionProvider">The connection provider implementation.</param>
    /// <returns>The current instance of <see cref="SqlServerInboxBuilder"/> to allow method chaining.</returns>
    public SqlServerInboxBuilder SetConnectionProvider(IAmARelationalDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    /// <summary>
    /// Builds and returns a configured instance of <see cref="MsSqlInbox"/>.
    /// </summary>
    /// <returns>A fully configured <see cref="MsSqlInbox"/> instance.</returns>
    /// <exception cref="ConfigurationException">Thrown if no configuration has been provided via <see cref="SetConfiguration"/>.</exception>
    internal MsSqlInbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("No configuration provided");
        }

        _connectionProvider ??= new MsSqlConnectionProvider(_configuration);
        return new MsSqlInbox(_configuration, _connectionProvider);
    }
}