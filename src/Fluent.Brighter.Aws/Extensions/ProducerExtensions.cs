using System;

using Fluent.Brighter.Aws;

using Paramore.Brighter.Locking.DynamoDb;
using Paramore.Brighter.MessagingGateway.AWSSQS;
using Paramore.Brighter.Outbox.DynamoDB;

namespace Fluent.Brighter;

public static class ProducerExtensions
{
    #region Publication
    public static ProducerBuilder AddSnsPublication(this ProducerBuilder builder,
        Action<SnsMessageProducerFactoryBuilder> configure)
    {
        var factory = new SnsMessageProducerFactoryBuilder();
        configure(factory);
        return builder.AddMessageProducerFactory(factory.Build());
    }
    
    public static ProducerBuilder AddSqsPublication(this ProducerBuilder builder,
        Action<SqsMessageProducerFactoryBuilder> configure)
    {
        var factory = new SqsMessageProducerFactoryBuilder();
        configure(factory);
        return builder.AddMessageProducerFactory(factory.Build());
    }
    #endregion

    #region Outbox
    public static ProducerBuilder UseDynamoDbOutbox(this ProducerBuilder builder,
        AWSMessagingGatewayConnection connection) => builder
        .UseDynamoDbOutbox(x => x.SetConnection(connection));

    public static ProducerBuilder UseDynamoDbOutbox(this ProducerBuilder builder,
        Action<DynamoDbOutboxBuilder> configure)
    {
        var outbox = new  DynamoDbOutboxBuilder();
        configure(outbox);
        return builder.UseDynamoDbOutbox(outbox.Build());
    }

    public static ProducerBuilder UseDynamoDbOutbox(this ProducerBuilder builder, DynamoDbOutbox outbox) 
        => builder.SetOutbox(outbox);

    #endregion

    #region Distributed Lock
    public static ProducerBuilder UseDynamoDbDistributedLock(this ProducerBuilder builder,
        Action<DynamoDbLockingProviderBuilder> configure)
    {
        var locker = new DynamoDbLockingProviderBuilder();
        configure(locker);
        return builder.UseDynamoDbDistributedLock(locker.Build());
    }

    public static ProducerBuilder UseDynamoDbDistributedLock(this ProducerBuilder builder, DynamoDbLockingProvider locker) =>
        builder.SetDistributedLock(locker);
    #endregion
}