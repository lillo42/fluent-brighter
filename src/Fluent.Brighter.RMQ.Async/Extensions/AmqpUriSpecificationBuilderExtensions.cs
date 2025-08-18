using System;

using Fluent.Brighter.RMQ.Async;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring AMQP URI specifications
/// </summary>
public static class AmqpUriSpecificationBuilderExtensions
{
    /// <summary>
    /// Sets the AMQP URI using a connection string
    /// </summary>
    /// <param name="builder">URI specification builder</param>
    /// <param name="uri">RabbitMQ connection string</param>
    /// <returns>Configured URI specification builder</returns>
    /// <remarks>
    /// The connection string should follow standard AMQP URI format:
    /// <code>amqp://username:password@hostname:port/virtualHost</code>
    /// Example: <c>amqp://guest:guest@localhost:5672/</c>
    /// </remarks>
    /// <example>
    /// builder.SetUri("amqp://user:pass@rabbitmq-server:5672/vhost")
    /// </example>
    public static AmqpUriSpecificationBuilder SetUri(
        this AmqpUriSpecificationBuilder builder, 
        string uri)
        => builder.SetUri(new Uri(uri, UriKind.RelativeOrAbsolute));
}