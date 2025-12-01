using System;

using Fluent.Brighter.AWS.V4;

using Paramore.Brighter.DynamoDb.V4;
using Paramore.Brighter.Locking.DynamoDB.V4;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;
using Paramore.Brighter.Outbox.DynamoDB.V4;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for ProducerBuilder to configure AWS-specific message producers,
/// outbox patterns, and distributed locking in Paramore.Brighter.
/// </summary>
public static class ProducerExtensions
{
    #region Publication

    /// <summary>
    /// Adds an SNS (Simple Notification Service) message producer using a builder pattern for fluent configuration.
    /// </summary>
    /// <param name="builder">The producer builder instance</param>
    /// <param name="configure">Action to configure the SNS message producer factory</param>
    /// <returns>The producer builder instance for method chaining</returns>
    public static ProducerBuilder AddSnsPublication(this ProducerBuilder builder,
        Action<SnsMessageProducerFactoryBuilder> configure)
    {
        var factory = new SnsMessageProducerFactoryBuilder();
        configure(factory);
        return builder.AddMessageProducerFactory(factory.Build());
    }

    /// <summary>
    /// Adds an SQS (Simple Queue Service) message producer using a builder pattern for fluent configuration.
    /// </summary>
    /// <param name="builder">The producer builder instance</param>
    /// <param name="configure">Action to configure the SQS message producer factory</param>
    /// <returns>The producer builder instance for method chaining</returns>
    public static ProducerBuilder AddSqsPublication(this ProducerBuilder builder,
        Action<SqsMessageProducerFactoryBuilder> configure)
    {
        var factory = new SqsMessageProducerFactoryBuilder();
        configure(factory);
        return builder.AddMessageProducerFactory(factory.Build());
    }
    #endregion

    #region Outbox

    /// <summary>
    /// Configures DynamoDB as the outbox store using a pre-configured AWS connection.
    /// </summary>
    /// <param name="builder">The producer builder instance</param>
    /// <param name="connection">Pre-configured AWS connection</param>
    /// <returns>The producer builder instance for method chaining</returns>
    public static ProducerBuilder UseDynamoDbOutbox(this ProducerBuilder builder,
        AWSMessagingGatewayConnection connection)
    {
        return builder
            .UseDynamoDbOutbox(x => x.SetConnection(connection));
    }

    /// <summary>
    /// Configures DynamoDB as the outbox store using a builder pattern for fluent configuration.
    /// </summary>
    /// <param name="builder">The producer builder instance</param>
    /// <param name="configure">Action to configure the DynamoDB outbox</param>
    /// <returns>The producer builder instance for method chaining</returns>
    public static ProducerBuilder UseDynamoDbOutbox(this ProducerBuilder builder,
        Action<DynamoDbOutboxBuilder> configure)
    {
        var outbox = new DynamoDbOutboxBuilder();
        configure(outbox);
        return builder.UseDynamoDbOutbox(outbox.Build());
    }

    /// <summary>
    /// Configures a pre-built DynamoDB outbox instance and sets up the necessary
    /// connection and transaction providers for unit of work pattern.
    /// </summary>
    /// <param name="builder">The producer builder instance</param>
    /// <param name="outbox">Pre-configured DynamoDB outbox</param>
    /// <returns>The producer builder instance for method chaining</returns>
    public static ProducerBuilder UseDynamoDbOutbox(this ProducerBuilder builder, DynamoDbOutbox outbox)
    {
        return builder.SetOutbox(outbox)
            .SetConnectionProvider(typeof(DynamoDbUnitOfWork))
            .SetTransactionProvider(typeof(DynamoDbUnitOfWork));
    }

    #endregion

    #region Distributed Lock
    /// <summary>
    /// Configures DynamoDB for distributed locking using a builder pattern for fluent configuration.
    /// </summary>
    /// <param name="builder">The producer builder instance</param>
    /// <param name="configure">Action to configure the DynamoDB locking provider</param>
    /// <returns>The producer builder instance for method chaining</returns>
    public static ProducerBuilder UseDynamoDbDistributedLock(this ProducerBuilder builder,
        Action<DynamoDbLockingProviderBuilder> configure)
    {
        var locker = new DynamoDbLockingProviderBuilder();
        configure(locker);
        return builder.UseDynamoDbDistributedLock(locker.Build());
    }

    /// <summary>
    /// Configures a pre-built DynamoDB distributed locking provider.
    /// </summary>
    /// <param name="builder">The producer builder instance</param>
    /// <param name="locker">Pre-configured DynamoDB locking provider</param>
    /// <returns>The producer builder instance for method chaining</returns>
    public static ProducerBuilder UseDynamoDbDistributedLock(this ProducerBuilder builder, DynamoDbLockingProvider locker)
    {
        return builder.SetDistributedLock(locker);
    }
    #endregion
}