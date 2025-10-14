using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.MsSql;

namespace Fluent.Brighter.SqlServer;

/// <summary>
/// Provides a fluent builder for configuring and creating an instance of <see cref="MsSqlMessageProducerFactory"/>,
/// which produces message producers that persist outgoing messages to a Microsoft SQL Server database
/// as part of the outbox pattern.
/// </summary>
public sealed class SqlServerMessageProducerFactoryBuilder
{
    private RelationalDatabaseConfiguration? _configuration;

    /// <summary>
    /// Sets the SQL Server database configuration used to connect to the outbox storage.
    /// </summary>
    /// <param name="connection">The relational database configuration containing connection string and optional settings.</param>
    /// <returns>The current instance of <see cref="SqlServerMessageProducerFactoryBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="connection"/> is null.</exception>
    public SqlServerMessageProducerFactoryBuilder SetConnection(RelationalDatabaseConfiguration connection)
    {
        _configuration = connection ?? throw new ArgumentNullException(nameof(connection));
        return this;
    }
    
    private readonly List<Publication> _publications = [];

    /// <summary>
    /// Adds a publication configuration that defines how messages of a specific type or route should be published.
    /// Multiple publications can be registered to support different message types or routing strategies.
    /// </summary>
    /// <param name="publication">The publication metadata, including routing key, CloudEvents attributes, and channel behavior.</param>
    /// <returns>The current instance of <see cref="SqlServerMessageProducerFactoryBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="publication"/> is null.</exception>
    public SqlServerMessageProducerFactoryBuilder AddPublication(Publication publication)
    {
        _publications.Add(publication ?? throw new ArgumentNullException(nameof(publication)));
        return this;
    }

    /// <summary>
    /// Builds and returns a configured instance of <see cref="MsSqlMessageProducerFactory"/>.
    /// </summary>
    /// <returns>A fully configured message producer factory for SQL Server.</returns>
    /// <exception cref="ConfigurationException">Thrown if no database configuration has been provided via <see cref="SetConnection"/>.</exception>
    internal MsSqlMessageProducerFactory Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("No configuration configured");
        }
        
        return new MsSqlMessageProducerFactory(_configuration, _publications);
    }
}