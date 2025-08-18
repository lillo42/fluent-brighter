using System;

using Fluent.Brighter.RMQ.Async;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for fluently configuring RabbitMQ message publications
/// </summary>
public static class RmqSubscriptionConfigurationExtensions
{
    /// <summary>
    /// Configures the RabbitMQ connection using a fluent builder
    /// </summary>
    /// <param name="builder">Producer factory builder</param>
    /// <param name="configure">Action to configure connection parameters</param>
    /// <returns>Configured producer factory builder</returns>
    /// <remarks>
    /// This method provides a streamlined way to configure both the connection
    /// and publications in a single fluent chain.
    /// </remarks>
    /// <example>
    /// var factoryBuilder = new RmqMessageProducerFactoryBuilder()
    ///     .SetConnection(conn => conn
    ///         .SetAmpq("amqp://localhost")
    ///         .SetExchange("app.events"))
    ///     .AddPublication(pub => pub
    ///         .SetTopic("order.created"));
    /// </example>
    public static RmqMessageProducerFactoryBuilder SetConnection(
        this RmqMessageProducerFactoryBuilder builder,
        Action<RmqMessagingGatewayConnectionBuilder> configure)
    {
        var conn = new  RmqMessagingGatewayConnectionBuilder();
        configure(conn);
        return builder.SetConnection(conn.Build());
    }
    
    /// <summary>
    /// Adds a publication configuration using a fluent builder
    /// </summary>
    /// <param name="builder">The producer factory builder</param>
    /// <param name="configure">Action to configure the publication using RmqPublicationBuilder</param>
    /// <returns>The producer factory builder for method chaining</returns>
    /// <example>
    /// builder.AddPublication(pub => pub
    ///     .SetTopic(new RoutingKey("events"))
    ///     .SetType(CloudEventsType.Event)
    /// );
    /// </example>
    public static RmqMessageProducerFactoryBuilder AddPublication(
        this RmqMessageProducerFactoryBuilder builder,
        Action<RmqPublicationBuilder> configure)
    {
        var publicationBuilder = new RmqPublicationBuilder();
        configure(publicationBuilder);
        return builder.AddPublication(publicationBuilder.Build());
    }
    
    /// <summary>
    /// Adds a strongly-typed publication configuration for a specific request type
    /// </summary>
    /// <typeparam name="TRequest">The message type implementing IRequest</typeparam>
    /// <param name="builder">The producer factory builder</param>
    /// <param name="configure">Action to configure the publication using RmqPublicationBuilder</param>
    /// <returns>The producer factory builder for method chaining</returns>
    /// <remarks>
    /// Automatically sets the request type before applying custom configurations.
    /// Recommended for typed messaging patterns.
    /// </remarks>
    /// <example>
    /// builder.AddPublication&lt;OrderEvent&gt;(pub => pub
    ///     .SetTopic(new RoutingKey("orders"))
    ///     .SetSource(new Uri("urn:services:orders"))
    /// );
    /// </example>
    public static RmqMessageProducerFactoryBuilder AddPublication<TRequest>(
        this RmqMessageProducerFactoryBuilder builder,
        Action<RmqPublicationBuilder> configure)
        where TRequest : class, IRequest
    {
        var publicationBuilder = new RmqPublicationBuilder();
        publicationBuilder.SetRequestType(typeof(TRequest));
        configure(publicationBuilder);
        return builder.AddPublication(publicationBuilder.Build());
    }
}