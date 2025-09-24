using System;
using System.Collections.Generic;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.AWS;

/// <summary>
/// Configurator class for managing and building SQS subscriptions in Paramore.Brighter.
/// Provides methods to add subscriptions either directly or through a builder pattern,
/// and supports both generic and specific subscription configuration.
/// </summary>
public sealed class SqsSubscriptionConfigurator(ChannelFactory channelFactory)
{
    internal List<SqsSubscription> Subscriptions { get; } = [];

    /// <summary>
    /// Adds a pre-built SQS subscription to the configuration.
    /// </summary>
    /// <param name="subscription">The pre-configured SQS subscription</param>
    /// <returns>The configurator instance for method chaining</returns>
    public SqsSubscriptionConfigurator AddSubscription(SqsSubscription subscription)
    {
        Subscriptions.Add(subscription);
        return this;
    }

    /// <summary>
    /// Adds an SQS subscription using a builder pattern for fluent configuration.
    /// </summary>
    /// <param name="configure">Action to configure the SQS subscription builder</param>
    /// <returns>The configurator instance for method chaining</returns>
    public SqsSubscriptionConfigurator AddSubscription(Action<SqsSubscriptionBuilder> configure)
    {
        var sub = new SqsSubscriptionBuilder();
        sub.SetChannelFactory(channelFactory);
        configure(sub);
        return AddSubscription(sub.Build());
    }

    /// <summary>
    /// Adds an SQS subscription for a specific request type using a builder pattern.
    /// Automatically sets the data type and provides sensible defaults based on the request type.
    /// </summary>
    /// <typeparam name="TRequest">The type of request message to subscribe to</typeparam>
    /// <param name="configure">Action to configure the SQS subscription builder</param>
    /// <returns>The configurator instance for method chaining</returns>
    public SqsSubscriptionConfigurator AddSubscription<TRequest>(Action<SqsSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        return AddSubscription(cfg =>
        {
            cfg.SetDataType(typeof(TRequest));
            configure(cfg);
        });
    }
}