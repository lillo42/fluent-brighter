using System.Collections.Generic;
using System.Linq;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.RocketMQ;

namespace Fluent.Brighter.RocketMQ;

/// <summary>
/// Fluent builder for configuring RocketMQ message producer factories in Brighter
/// </summary>
/// <remarks>
/// Constructs the factory that creates RocketMQ message producers. Requires:
/// 1. A valid connection configuration (SetConnection)
/// 2. At least one publication configuration (via SetPublications or AddPublication)
/// </remarks>
public sealed class RocketMessageProducerFactoryBuilder
{
    private RocketMessagingGatewayConnection? _connection;

    /// <summary>
    /// Sets the RocketMQ connection configuration
    /// </summary>
    /// <param name="connection">Pre-configured gateway connection settings</param>
    /// <returns>This builder for fluent chaining</returns>
    public RocketMessageProducerFactoryBuilder SetConnection(RocketMessagingGatewayConnection connection)
    {
        _connection = connection;
        return this;
    }
    
    private List<RocketMqPublication> _publications = [];

    /// <summary>
    /// Configures the collection of message publications (replaces existing)
    /// </summary>
    /// <param name="publications">Array of publication configurations</param>
    /// <returns>This builder for fluent chaining</returns>
    public RocketMessageProducerFactoryBuilder SetPublications(params RocketMqPublication[] publications)
    {
        _publications = publications.ToList();
        return this;
    }

    /// <summary>
    /// Adds a single message publication configuration
    /// </summary>
    /// <param name="publication">Publication configuration to add</param>
    /// <returns>This builder for fluent chaining</returns>
    public RocketMessageProducerFactoryBuilder AddPublication(RocketMqPublication publication)
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
    internal RocketMessageProducerFactory Build()
    {
        if (_connection == null)
        {
            throw new ConfigurationException("RocketMQ connection configuration is required. Use SetConnection() to provide gateway settings.");
        }
        
        return new RocketMessageProducerFactory(_connection, _publications);
    }
}