using System;

using Fluent.Brighter.AWS;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for SnsMessageProducerFactoryBuilder to provide additional configuration options
/// and convenience methods for creating SNS message producer factories.
/// </summary>
public static class SnsMessageProducerFactoryBuilderExtensions
{
    /// <summary>
    /// Sets the AWS connection configuration using a builder pattern for the SNS message producer factory.
    /// </summary>
    /// <param name="builder">The SNS message producer factory builder instance</param>
    /// <param name="configure">Action to configure the AWS connection builder</param>
    /// <returns>The SNS message producer factory builder instance for method chaining</returns>
    public static SnsMessageProducerFactoryBuilder SetConfiguration(this SnsMessageProducerFactoryBuilder builder,
        Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var factory = new AWSMessagingGatewayConnectionBuilder();
        configure(factory);
        return builder.SetConfiguration(factory.Build());
    }

    /// <summary>
    /// Adds an SNS publication for a specific request type using a builder pattern for fluent configuration.
    /// Automatically sets the request type based on the generic parameter.
    /// </summary>
    /// <typeparam name="TRequest">The type of request message to publish</typeparam>
    /// <param name="builder">The SNS message producer factory builder instance</param>
    /// <param name="configure">Action to configure the SNS publication</param>
    /// <returns>The SNS message producer factory builder instance for method chaining</returns>
    public static SnsMessageProducerFactoryBuilder AddPublication<TRequest>(
        this SnsMessageProducerFactoryBuilder builder,
        Action<SnsPublicationBuilder> configure)
        where TRequest : class, IRequest
    {
        var factory = new SnsPublicationBuilder();
        factory.SetRequestType(typeof(TRequest));
        configure(factory);
        return builder.AddPublication(factory.Build());
    }

    /// <summary>
    /// Adds an SNS publication using a builder pattern for fluent configuration.
    /// </summary>
    /// <param name="builder">The SNS message producer factory builder instance</param>
    /// <param name="configure">Action to configure the SNS publication</param>
    /// <returns>The SNS message producer factory builder instance for method chaining</returns>
    public static SnsMessageProducerFactoryBuilder AddPublication(this SnsMessageProducerFactoryBuilder builder,
        Action<SnsPublicationBuilder> configure)
    {
        var factory = new SnsPublicationBuilder();
        configure(factory);
        return builder.AddPublication(factory.Build());
    }
}