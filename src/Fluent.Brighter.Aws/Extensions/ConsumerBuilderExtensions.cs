using System;

using Amazon.Runtime.Internal;

using Fluent.Brighter.Aws;

using Paramore.Brighter.Inbox.DynamoDB;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for configuring AWS-specific consumers in Paramore.Brighter.
/// Provides fluent methods to add SQS subscriptions, AWS channel factories, and DynamoDB inbox configurations
/// to the ConsumerBuilder.
/// </summary>
public static class ConsumerBuilderExtensions
{
    /// <summary>
    /// Adds a pre-configured SQS subscription to the consumer builder.
    /// </summary>
    /// <param name="builder">The consumer builder instance</param>
    /// <param name="subscription">Pre-configured SQS subscription</param>
    /// <returns>The consumer builder instance for method chaining</returns>
    public static ConsumerBuilder AddSqsSubscription(this ConsumerBuilder builder, SqsSubscription subscription)
        => builder.AddSubscription(subscription);

    /// <summary>
    /// Adds an SQS subscription using a builder pattern for fluent configuration.
    /// </summary>
    /// <param name="builder">The consumer builder instance</param>
    /// <param name="configure">Action to configure the SQS subscription</param>
    /// <returns>The consumer builder instance for method chaining</returns>
    public static ConsumerBuilder AddSqsSubscription(this ConsumerBuilder builder,
        Action<SqsSubscriptionBuilder> configure)
    {
        var subscription = new SqsSubscriptionBuilder();
        configure(subscription);
        return builder.AddSubscription(subscription.Build());
    }
    
    /// <summary>
    /// Adds an SQS subscription for a specific request type using a builder pattern.
    /// Automatically sets the data type based on the generic parameter.
    /// </summary>
    /// <typeparam name="TRequest">The type of request message to subscribe to</typeparam>
    /// <param name="builder">The consumer builder instance</param>
    /// <param name="configure">Action to configure the SQS subscription</param>
    /// <returns>The consumer builder instance for method chaining</returns>
    public static ConsumerBuilder AddSqsSubscription<TRequest>(this ConsumerBuilder builder, Action<SqsSubscriptionBuilder> configure)
        where TRequest : class, IRequest
    {
        var subscription = new SqsSubscriptionBuilder();
        subscription.SetDataType(typeof(TRequest));
        configure(subscription);
        return builder.AddSubscription(subscription.Build());
    }
    
    /// <summary>
    /// Adds an AWS channel factory using a pre-configured AWS connection.
    /// </summary>
    /// <param name="builder">The consumer builder instance</param>
    /// <param name="configure">Pre-configured AWS connection</param>
    /// <returns>The consumer builder instance for method chaining</returns>
    public static ConsumerBuilder AddAwsChannelFactory(this ConsumerBuilder builder, AWSMessagingGatewayConnection configure) 
        => builder.AddChannelFactory(new ChannelFactory(configure));

    /// <summary>
    /// Adds an AWS channel factory using a builder pattern for fluent connection configuration.
    /// </summary>
    /// <param name="builder">The consumer builder instance</param>
    /// <param name="configure">Action to configure the AWS connection</param>
    /// <returns>The consumer builder instance for method chaining</returns>
    public static ConsumerBuilder AddAwsChannelFactory(this ConsumerBuilder builder, Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var subscription = new AWSMessagingGatewayConnectionBuilder();
        configure(subscription);
        return builder.AddChannelFactory(new ChannelFactory(subscription.Build()));
    }

    /// <summary>
    /// Configures a DynamoDB inbox using a builder pattern for fluent configuration.
    /// </summary>
    /// <param name="builder">The consumer builder instance</param>
    /// <param name="configure">Action to configure the DynamoDB inbox</param>
    /// <returns>The consumer builder instance for method chaining</returns>
    public static ConsumerBuilder UseDynamoDbInbox(this ConsumerBuilder builder, Action<DynamoDbInboxBuilder> configure)
    {
        var configuration = new DynamoDbInboxBuilder();
        configure(configuration);
        return builder.UseDynamoDbInbox(configuration.Build());
    }
    
    /// <summary>
    /// Configures a DynamoDB inbox using a pre-configured AWS connection.
    /// </summary>
    /// <param name="builder">The consumer builder instance</param>
    /// <param name="connection">Pre-configured AWS connection</param>
    /// <returns>The consumer builder instance for method chaining</returns>
    public static ConsumerBuilder UseDynamoDbInbox(this ConsumerBuilder builder, AWSMessagingGatewayConnection connection)
    {
        return builder.UseDynamoDbInbox(cfg => cfg.SetConnection(connection));
    }

    /// <summary>
    /// Configures a pre-built DynamoDB inbox instance.
    /// </summary>
    /// <param name="builder">The consumer builder instance</param>
    /// <param name="inbox">Pre-configured DynamoDB inbox</param>
    /// <returns>The consumer builder instance for method chaining</returns>
    public static ConsumerBuilder UseDynamoDbInbox(this ConsumerBuilder builder, DynamoDbInbox inbox) 
        => builder.SetInbox(cfg => cfg.SetInbox(inbox));
}