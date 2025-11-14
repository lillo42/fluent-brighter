using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter.Postgres;

/// <summary>
/// Builder class for configuring and creating a PostgreSQL message producer factory.
/// This factory is responsible for creating message producers that publish messages to PostgreSQL-based messaging gateway.
/// </summary>
public sealed class PostgresMessageProducerFactoryBuilder
{
    private PostgresMessagingGatewayConnection? _connection;

    /// <summary>
    /// Sets the PostgreSQL messaging gateway connection to be used by the message producers.
    /// </summary>
    /// <param name="connection">The PostgreSQL messaging gateway connection containing database connection details.</param>
    /// <returns>The current <see cref="PostgresMessageProducerFactoryBuilder"/> instance for method chaining.</returns>
    public PostgresMessageProducerFactoryBuilder SetConnection(PostgresMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }
    
    private readonly List<PostgresPublication> _publications = [];
    
    /// <summary>
    /// Adds a publication configuration that defines how messages are published to PostgreSQL.
    /// Publications define the mapping between message types and PostgreSQL topics/tables.
    /// </summary>
    /// <param name="publication">The publication configuration to add.</param>
    /// <returns>The current <see cref="PostgresMessageProducerFactoryBuilder"/> instance for method chaining.</returns>
    public PostgresMessageProducerFactoryBuilder AddPublication(PostgresPublication publication)
    {
        _publications.Add(publication);
        return this;
    }

    /// <summary>
    /// Builds and returns a configured <see cref="PostgresMessageProducerFactory"/> instance.
    /// This method is called internally to create the factory with the configured connection and publications.
    /// </summary>
    /// <returns>A configured <see cref="PostgresMessageProducerFactory"/> instance.</returns>
    /// <exception cref="ConfigurationException">Thrown when no connection has been configured via <see cref="SetConnection"/>.</exception>
    internal PostgresMessageProducerFactory Build()
    {
        if (_connection == null)
        {
            throw new ConfigurationException("No connection configured");
        }
        
        return new PostgresMessageProducerFactory(_connection, _publications);
    }
}