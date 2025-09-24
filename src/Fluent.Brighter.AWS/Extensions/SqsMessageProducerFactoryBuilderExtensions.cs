using System;

using Fluent.Brighter.AWS;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for SqsMessageProducerFactoryBuilder to provide additional configuration options
/// and convenience methods for creating SQS message producer factories.
/// </summary>
public static class SqsMessageProducerFactoryBuilderExtensions
{
    /// <summary>
    /// Sets the AWS connection configuration using a builder pattern for the SQS message producer factory.
    /// </summary>
    /// <param name="builder">The SQS message producer factory builder instance</param>
    /// <param name="configure">Action to configure the AWS connection builder</param>
    /// <returns>The SQS message producer factory builder instance for method chaining</returns>
    public static SqsMessageProducerFactoryBuilder SetConfiguration(this SqsMessageProducerFactoryBuilder builder,
        Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var factory = new AWSMessagingGatewayConnectionBuilder();
        configure(factory);
        return builder.SetConfiguration(factory.Build());
    }

    /// <summary>
    /// Adds an SQS publication for a specific request type using a builder pattern for fluent configuration.
    /// Automatically sets the request type based on the generic parameter.
    /// </summary>
    /// <typeparam name="TRequest">The type of request message to publish</typeparam>
    /// <param name="builder">The SQS message producer factory builder instance</param>
    /// <param name="configure">Action to configure the SQS publication</param>
    /// <returns>The SQS message producer factory builder instance for method chaining</returns>
    public static SqsMessageProducerFactoryBuilder AddPublication<TRequest>(
        this SqsMessageProducerFactoryBuilder builder,
        Action<SqsPublicationBuilder> configure)
    {
        var factory = new SqsPublicationBuilder();
        factory.SetRequestType(typeof(TRequest));
        configure(factory);
        return builder.AddPublication(factory.Build());
    }

    /// <summary>
    /// Adds an SQS publication using a builder pattern for fluent configuration.
    /// </summary>
    /// <param name="builder">The SQS message producer factory builder instance</param>
    /// <param name="configure">Action to configure the SQS publication</param>
    /// <returns>The SQS message producer factory builder instance for method chaining</returns>
    public static SqsMessageProducerFactoryBuilder AddPublication(this SqsMessageProducerFactoryBuilder builder,
        Action<SqsPublicationBuilder> configure)
    {
        var factory = new SqsPublicationBuilder();
        configure(factory);
        return builder.AddPublication(factory.Build());
    }
}