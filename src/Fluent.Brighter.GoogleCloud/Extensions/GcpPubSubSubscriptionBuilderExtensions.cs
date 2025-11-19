using System;

using Fluent.Brighter.GoogleCloud;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.GcpPubSub;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring Google Cloud Pub/Sub subscriptions using a fluent API.
/// </summary>
public static class GcpPubSubSubscriptionBuilderExtensions
{
    /// <summary>
    /// Configures the Google Cloud Pub/Sub topic attributes using a fluent builder.
    /// This allows setting properties like project ID, topic name, labels, retention duration,
    /// schema settings, and encryption configuration for the topic associated with this subscription.
    /// </summary>
    /// <param name="builder">The GCP Pub/Sub subscription builder instance</param>
    /// <param name="configure">An action to configure the topic attributes using the builder</param>
    /// <returns>The configured <see cref="GcpPubSubSubscriptionBuilder"/> instance for method chaining</returns>
    /// <example>
    /// <code>
    /// builder.SetTopicAttributes(attrs =>
    /// {
    ///     attrs.SetName("my-topic")
    ///          .SetProjectId("my-project")
    ///          .AddLabel("environment", "production");
    /// });
    /// </code>
    /// </example>
    public static GcpPubSubSubscriptionBuilder SetTopicAttributes(this GcpPubSubSubscriptionBuilder builder,
        Action<TopicAttributeBuilder> configure)
    {
        var attrs = new TopicAttributeBuilder();
        configure(attrs);
        return builder.SetTopicAttributes(attrs.Build());
    }

    /// <summary>
    /// Enables message ordering for the subscription.
    /// When enabled, messages published to the topic are delivered in the order they were published,
    /// provided they were published with an ordering key.
    /// </summary>
    /// <param name="builder">The GCP Pub/Sub subscription builder instance</param>
    /// <returns>The configured <see cref="GcpPubSubSubscriptionBuilder"/> instance for method chaining</returns>
    public static GcpPubSubSubscriptionBuilder EnableMessageOrdering(this GcpPubSubSubscriptionBuilder builder)
    {
        return builder.SetEnableMessageOrdering(true);
    }
    
    /// <summary>
    /// Disables message ordering for the subscription.
    /// Messages will be delivered in the order received by Pub/Sub, which may not match publishing order.
    /// </summary>
    /// <param name="builder">The GCP Pub/Sub subscription builder instance</param>
    /// <returns>The configured <see cref="GcpPubSubSubscriptionBuilder"/> instance for method chaining</returns>
    public static GcpPubSubSubscriptionBuilder DisableMessageOrdering(this GcpPubSubSubscriptionBuilder builder)
    {
        return builder.SetEnableMessageOrdering(false);
    }
    
    /// <summary>
    /// Enables exactly-once delivery for the subscription.
    /// When enabled, Pub/Sub guarantees that each message is delivered and acknowledged exactly once,
    /// preventing duplicate processing. This feature may increase latency and cost.
    /// </summary>
    /// <param name="builder">The GCP Pub/Sub subscription builder instance</param>
    /// <returns>The configured <see cref="GcpPubSubSubscriptionBuilder"/> instance for method chaining</returns>
    public static GcpPubSubSubscriptionBuilder EnableExactlyOnceDelivery(this GcpPubSubSubscriptionBuilder builder)
    {
        return builder.SetEnableExactlyOnceDelivery(true);
    }
    
    /// <summary>
    /// Disables exactly-once delivery for the subscription.
    /// Messages may be delivered more than once (at-least-once delivery semantics).
    /// This is the default behavior and offers better performance and lower cost.
    /// </summary>
    /// <param name="builder">The GCP Pub/Sub subscription builder instance</param>
    /// <returns>The configured <see cref="GcpPubSubSubscriptionBuilder"/> instance for method chaining</returns>
    public static GcpPubSubSubscriptionBuilder DisableExactlyOnceDelivery(this GcpPubSubSubscriptionBuilder builder)
    {
        return builder.SetEnableExactlyOnceDelivery(false);
    }

    /// <summary>
    /// Configures the subscription to use streaming mode for message consumption.
    /// In streaming mode, Pub/Sub pushes messages to the subscriber as they become available,
    /// providing lower latency. This is the default and recommended mode for most use cases.
    /// </summary>
    /// <param name="builder">The GCP Pub/Sub subscription builder instance</param>
    /// <returns>The configured <see cref="GcpPubSubSubscriptionBuilder"/> instance for method chaining</returns>
    public static GcpPubSubSubscriptionBuilder UseStreamMode(this GcpPubSubSubscriptionBuilder builder)
    {
        return builder.SetSubscriptionMode(SubscriptionMode.Stream);
    }
    
    /// <summary>
    /// Configures the subscription to use pull mode for message consumption.
    /// In pull mode, the subscriber explicitly requests messages from Pub/Sub,
    /// providing more control over message flow and processing rate.
    /// </summary>
    /// <param name="builder">The GCP Pub/Sub subscription builder instance</param>
    /// <returns>The configured <see cref="GcpPubSubSubscriptionBuilder"/> instance for method chaining</returns>
    public static GcpPubSubSubscriptionBuilder UsePullMode(this GcpPubSubSubscriptionBuilder builder)
    {
        return builder.SetSubscriptionMode(SubscriptionMode.Pull);
    }

    /// <summary>
    /// Configures the Dead Letter Policy (DLQ) for the subscription using a fluent builder.
    /// Messages that fail processing after a specified number of delivery attempts will be
    /// forwarded to the configured dead letter topic for manual inspection or reprocessing.
    /// </summary>
    /// <param name="builder">The GCP Pub/Sub subscription builder instance</param>
    /// <param name="configure">An action to configure the dead letter policy using the builder</param>
    /// <returns>The configured <see cref="GcpPubSubSubscriptionBuilder"/> instance for method chaining</returns>
    /// <example>
    /// <code>
    /// builder.SetDeadLetter(dlq =>
    /// {
    ///     dlq.SetTopicName(new RoutingKey("my-dlq-topic"))
    ///        .SetSubscription(new ChannelName("my-subscription"))
    ///        .SetMaxDeliveryAttempts(5);
    /// });
    /// </code>
    /// </example>
    public static GcpPubSubSubscriptionBuilder SetDeadLetter(this GcpPubSubSubscriptionBuilder builder,
        Action<DeadLetterPolicyBuilder> configure)
    {
        var policy = new DeadLetterPolicyBuilder();
        configure(policy);
        return builder.SetDeadLetter(policy.Build());
    }
    
    /// <summary>
    /// Configures the subscription to use the Proactor message pump type for asynchronous message processing.
    /// The Proactor pattern processes messages using async/await, allowing for concurrent message handling
    /// and better resource utilization. This is the recommended mode for async pipelines.
    /// </summary>
    /// <param name="builder">The GCP Pub/Sub subscription builder instance</param>
    /// <returns>The configured <see cref="GcpPubSubSubscriptionBuilder"/> instance for method chaining</returns>
    public static GcpPubSubSubscriptionBuilder UseProactor(this GcpPubSubSubscriptionBuilder builder)
    {
        return builder.SetMessagePumpType(MessagePumpType.Proactor);
    }

    /// <summary>
    /// Configures the subscription to use the Reactor message pump type for synchronous message processing.
    /// The Reactor pattern processes messages synchronously in a single-threaded manner.
    /// Use this mode for synchronous pipelines or when you need predictable, sequential processing.
    /// </summary>
    /// <param name="builder">The GCP Pub/Sub subscription builder instance</param>
    /// <returns>The configured <see cref="GcpPubSubSubscriptionBuilder"/> instance for method chaining</returns>
    public static GcpPubSubSubscriptionBuilder UseReactor(this GcpPubSubSubscriptionBuilder builder)
    {
        return builder.SetMessagePumpType(MessagePumpType.Reactor);
    }
}