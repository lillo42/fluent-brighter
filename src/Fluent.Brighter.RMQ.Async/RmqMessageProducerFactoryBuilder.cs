using System.Collections.Generic;
using System.Linq;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RMQ.Async;

namespace Fluent.Brighter.RMQ.Async;

/// <summary>
/// Fluent builder for configuring RabbitMQ message producer factories in Brighter
/// </summary>
/// <remarks>
/// Constructs the factory that creates RabbitMQ message producers. Requires:
/// 1. A valid connection configuration (SetConnection)
/// 2. At least one publication configuration (via SetPublications or AddPublication)
/// </remarks>
public sealed class RmqMessageProducerFactoryBuilder
{
    private RmqMessagingGatewayConnection? _connection;

    /// <summary>
    /// Sets the RabbitMQ connection configuration
    /// </summary>
    /// <param name="connection">Pre-configured gateway connection settings</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqMessageProducerFactoryBuilder SetConnection(RmqMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }
    
    private List<RmqPublication> _publications = [];

    /// <summary>
    /// Configures the collection of message publications (replaces existing)
    /// </summary>
    /// <param name="publications">Array of publication configurations</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqMessageProducerFactoryBuilder SetPublications(params RmqPublication[] publications)
    {
        _publications = publications.ToList();
        return this;
    }

    /// <summary>
    /// Adds a single message publication configuration
    /// </summary>
    /// <param name="publication">Publication configuration to add</param>
    /// <returns>This builder for fluent chaining</returns>
    public RmqMessageProducerFactoryBuilder AddPublication(RmqPublication publication)
    {
        _publications.Add(publication);
        return this;
    }

    /// <summary>
    /// Constructs the message producer factory
    /// </summary>
    /// <returns>Configured producer factory</returns>
    /// <exception cref="ConfigurationException">
    /// Thrown if connection is missing or no publications are configured
    /// </exception>
    internal RmqMessageProducerFactory Build()
    {
        if (_connection == null)
        {
            throw new ConfigurationException("RabbitMQ connection configuration is required. Use SetConnection() to provide gateway settings.");
        }
        
        return new RmqMessageProducerFactory(_connection, _publications);
    }
}