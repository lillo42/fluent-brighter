using System;

using Amazon.DynamoDBv2;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Paramore.Brighter;
using Paramore.Brighter.DynamoDb.V4;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;

namespace Fluent.Brighter.AWS.V4;

/// <summary>
/// Central configuration class for setting up AWS-related services in Paramore.Brighter.
/// Provides fluent methods to configure connections, message producers (SNS/SQS), subscriptions,
/// inbox/outbox patterns with DynamoDB, distributed locking, and S3-based luggage storage.
/// </summary>
public sealed class AWSConfigurator
{
    private AWSMessagingGatewayConnection? _connection;
    private Action<FluentBrighterBuilder> _action = static _ => { };

    /// <summary>
    /// Sets the AWS connection configuration using a builder pattern.
    /// </summary>
    /// <param name="configure">Action to configure the AWS connection builder</param>
    /// <returns>The configurator instance for method chaining</returns>
    public AWSConfigurator SetConnection(Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new AWSMessagingGatewayConnectionBuilder();
        configure(connection);
        _connection = connection.Build();
        return this;
    }

    /// <summary>
    /// Sets the AWS connection configuration directly with a pre-built connection.
    /// </summary>
    /// <param name="configure">Pre-configured AWS connection</param>
    /// <returns>The configurator instance for method chaining</returns>
    public AWSConfigurator SetConnection(AWSMessagingGatewayConnection configure)
    {
        _connection = configure;
        return this;
    }

    /// <summary>
    /// Configures SNS (Simple Notification Service) for message publication.
    /// </summary>
    /// <param name="configure">Action to configure SNS publication settings</param>
    /// <returns>The configurator instance for method chaining</returns>
    public AWSConfigurator UseSnsPublication(Action<SnsMessageProducerFactoryBuilder> configure)
    {
        _action += fluent => fluent
            .Producers(producer => producer.AddSnsPublication(cfg =>
            {
                cfg.SetConfiguration(_connection!);
                configure(cfg);
            }));

        return this;
    }

    /// <summary>
    /// Configures SQS (Simple Queue Service) for message publication.
    /// </summary>
    /// <param name="configure">Action to configure SQS publication settings</param>
    /// <returns>The configurator instance for method chaining</returns>
    public AWSConfigurator UseSqsPublication(Action<SqsMessageProducerFactoryBuilder> configure)
    {
        _action += fluent => fluent
            .Producers(producer => producer.AddSqsPublication(cfg =>
            {
                cfg.SetConfiguration(_connection!);
                configure(cfg);
            }));

        return this;
    }

    /// <summary>
    /// Configures SQS subscriptions for message consumption.
    /// </summary>
    /// <param name="configure">Action to configure SQS subscription settings</param>
    /// <returns>The configurator instance for method chaining</returns>
    public AWSConfigurator UseSqsSubscription(Action<SqsSubscriptionConfigurator> configure)
    {
        _action += fluent => fluent
            .Subscriptions(sub =>
            {
                var channel = new ChannelFactory(_connection!);
                var configurator = new SqsSubscriptionConfigurator(channel);
                configure(configurator);

                sub.AddChannelFactory(channel);
                foreach (var subscription in configurator.Subscriptions)
                {
                    sub.AddSqsSubscription(subscription);
                }
            });

        return this;
    }

    #region Inbox 

    /// <summary>
    /// Configures DynamoDB as the inbox store using default settings.
    /// </summary>
    /// <returns>The configurator instance for method chaining</returns>
    public AWSConfigurator UseDynamoDbInbox()
    {
        _action += fluent => fluent.Subscriptions(s => s.UseDynamoDbInbox(_connection!));
        return this;
    }

    /// <summary>
    /// Configures DynamoDB as the inbox store with a specific table name.
    /// </summary>
    /// <param name="tableName">Name of the DynamoDB table to use</param>
    /// <returns>The configurator instance for method chaining</returns>
    public AWSConfigurator UseDynamoDbInbox(string tableName)
    {
        _action += fluent => fluent.Subscriptions(x => x
            .UseDynamoDbInbox(cfg => cfg
                .SetConnection(_connection!)
                .SetTableName(tableName)));
        return this;
    }
    #endregion

    #region Outbox

    /// <summary>
    /// Configures DynamoDB as the outbox store using default settings.
    /// </summary>
    /// <returns>The configurator instance for method chaining</returns>
    public AWSConfigurator UseDynamoDbOutbox()
    {
        return UseDynamoDbOutbox(static _ => { });
    }

    /// <summary>
    /// Configures DynamoDB as the outbox store with a specific table name.
    /// </summary>
    /// <param name="tableName">Name of the DynamoDB table to use</param>
    /// <returns>The configurator instance for method chaining</returns>
    public AWSConfigurator UseDynamoDbOutbox(string tableName)
    {
        return UseDynamoDbOutbox(c => c.SetTableName(tableName));
    }

    /// <summary>
    /// Configures DynamoDB as the outbox store with custom settings.
    /// </summary>
    /// <param name="configure">Action to configure DynamoDB outbox settings</param>
    /// <returns>The configurator instance for method chaining</returns>
    public AWSConfigurator UseDynamoDbOutbox(Action<DynamoDbOutboxConfigurationBuilder> configure)
    {
        _action += fluent =>
            {
                fluent
                    .Producers(p => p
                        .UseDynamoDbOutbox(o => o
                            .SetConnection(_connection!)
                            .SetConfiguration(configure)));

                fluent.RegisterServices(services =>
                {
                    var config = new AmazonDynamoDBConfig { RegionEndpoint = _connection!.Region };
                    _connection.ClientConfigAction?.Invoke(config);

                    services.TryAddSingleton<IAmazonDynamoDB>(new AmazonDynamoDBClient(_connection.Credentials, config));
                    services.AddSingleton<DynamoDbUnitOfWork>()
                        .AddSingleton<IAmADynamoDbTransactionProvider>(provider => provider.GetRequiredService<DynamoDbUnitOfWork>());
                });
            };
        return this;
    }

    /// <summary>
    /// Configures DynamoDB as the outbox archive store using default settings.
    /// </summary>
    /// <returns>The configurator instance for method chaining</returns>
    public AWSConfigurator UseDynamoDbOutboxArchive()
    {
        _action += static fluent => fluent.UseDynamoDbTransactionOutboxArchive();
        return this;
    }

    /// <summary>
    /// Configures DynamoDB as the outbox archive store with custom settings.
    /// </summary>
    /// <param name="configure">Action to configure outbox archiver options</param>
    /// <returns>The configurator instance for method chaining</returns>
    public AWSConfigurator UseDynamoDbOutboxArchive(Action<TimedOutboxArchiverOptionsBuilder> configure)
    {
        _action += fluent => fluent.UseDynamoDbTransactionOutboxArchive(configure);
        return this;
    }
    #endregion

    #region Distributed Lock

    /// <summary>
    /// Configures DynamoDB for distributed locking with custom settings.
    /// </summary>
    /// <param name="configure">Action to configure distributed locking options</param>
    /// <returns>The configurator instance for method chaining</returns>
    public AWSConfigurator UseDynamoDbDistributedLock(Action<DynamoDbLockingProviderOptionsBuilder> configure)
    {
        var options = new DynamoDbLockingProviderOptionsBuilder();
        configure(options);
        _action += fluent => fluent
            .Producers(l => l.UseDynamoDbDistributedLock(cfg => cfg
                .SetConnection(_connection!)
                .SetConfiguration(configure)));

        return this;
    }

    #endregion

    #region S3

    /// <summary>
    /// Configures S3 as the luggage store with a specific bucket name.
    /// </summary>
    /// <param name="bucketName">Name of the S3 bucket to use</param>
    /// <returns>The configurator instance for method chaining</returns>
    public AWSConfigurator UseS3LuggageStore(string bucketName)
    {
        return UseS3LuggageStore(cfg => cfg.SetBucketName(bucketName));
    }

    /// <summary>
    /// Configures S3 as the luggage store with custom settings.
    /// </summary>
    /// <param name="configure">Action to configure S3 luggage store settings</param>
    /// <returns>The configurator instance for method chaining</returns>
    public AWSConfigurator UseS3LuggageStore(Action<S3LuggageStoreBuilder> configure)
    {
        _action += fluent => fluent
            .SetLuggageStore(store => store
                .UseS3LuggageStore(cfg =>
                {
                    cfg.SetConnection(_connection!);
                    configure(cfg);
                }));
        return this;
    }

    #endregion

    #region Scheduler

    /// <summary>
    /// Configures AWS EventBridge Scheduler for delayed message delivery using default settings.
    /// </summary>
    /// <returns>The configurator instance for method chaining</returns>
    /// <remarks>
    /// Enables scheduling of messages for future delivery using AWS EventBridge Scheduler.
    /// Uses default configuration with the provided AWS connection.
    /// </remarks>
    public AWSConfigurator UseScheduler()
    {
        _action += fluent => fluent
            .SetScheduler(scheduler => scheduler
                    .UseAWSScheduler(opt => opt
                        .SetConnection(_connection!)));
        return this;
    }

    /// <summary>
    /// Configures AWS EventBridge Scheduler for delayed message delivery with custom settings.
    /// </summary>
    /// <param name="configure">Action to configure scheduler factory settings</param>
    /// <returns>The configurator instance for method chaining</returns>
    /// <remarks>
    /// Enables scheduling of messages for future delivery using AWS EventBridge Scheduler
    /// with custom configuration for scheduling behavior, conflict resolution, and targeting.
    /// </remarks>
    public AWSConfigurator UseScheduler(Action<SchedulerFactoryBuilder> configure)
    {
        _action += fluent => fluent
            .SetScheduler(scheduler => scheduler
                .UseAWSScheduler(opt =>
                {
                    opt.SetConnection(_connection!);
                    configure(opt);
                }));
        return this;
    }

    #endregion

    internal void SetFluentBrighter(FluentBrighterBuilder builder)
    {
        if (_connection == null)
        {
            throw new ConfigurationException("No MessagingGatewayConnection was set");
        }

        _action(builder);
    }
}