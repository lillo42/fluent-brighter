using System;

namespace Fluent.Brighter.RMQ.Async;

/// <summary>
/// Provides extension methods for integrating RabbitMQ with Brighter service configuration.
/// Enables fluent configuration of RabbitMQ-based messaging infrastructure.
/// </summary>
public static class BrighterRegisterExtensions
{
    /// <summary>
    /// Configures RabbitMQ as the messaging infrastructure for Brighter.
    /// </summary>
    /// <param name="configurator">The Brighter configurator instance to extend.</param>
    /// <param name="configure">An action to configure RabbitMQ-specific settings through <see cref="RmqConfigurator"/>.</param>
    /// <returns>The original configurator instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is null.</exception>
    /// <example>
    /// Usage example:
    /// <code>
    /// services.AddBrighter(config => 
    ///     config.UsingRabbitMQ(rmq => 
    ///         rmq.ConnectWith("amqp://localhost:5672")
    ///            .UseExchange("my-exchange")
    ///            .UseCircuitBreaker(30000)
    ///     )
    /// );
    /// </code>
    /// </example>

    public static IBrighterConfigurator UsingRabbitMQ(this IBrighterConfigurator configurator, Action<RmqConfigurator> configure)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        var rmqConfigurator = new RmqConfigurator();
        configure(rmqConfigurator);
        return rmqConfigurator.AddRabbitMq(configurator);
    }
}