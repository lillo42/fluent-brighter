using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.Aws;

public sealed class AwsConfigurator
{
    private AWSMessagingGatewayConnection? _connection;
    private Action<FluentBrighterBuilder> _action = _ => { };

    public AwsConfigurator SetConnection(Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new AWSMessagingGatewayConnectionBuilder();
        configure(connection);
        _connection = connection.Build();
        return this;
    }

    public AwsConfigurator SetConnection(AWSMessagingGatewayConnection configure)
    {
        _connection = configure;
        return this;
    }

    public AwsConfigurator UseSnsPublication(Action<SnsMessageProducerFactoryBuilder> configure)
    {
        _action += fluent =>
        {
            fluent.Producers(producer => producer.AddSnsPublication(cfg =>
            {
                cfg.SetConfiguration(_connection!);
                configure(cfg);
            }));
        };

        return this;
    }
    
    public AwsConfigurator UseSqsPublication(Action<SqsMessageProducerFactoryBuilder> configure)
    {
        _action += fluent =>
        {
            fluent.Producers(producer => producer.AddSqsPublication(cfg =>
            {
                cfg.SetConfiguration(_connection!);
                configure(cfg);
            }));
        };

        return this;
    }

    public AwsConfigurator UseSqsSubscription(Action<SqsSubscriptionConfigurator> configure)
    {
        _action += fluent =>
        {
            fluent.Subscriptions(sub =>
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
        };

        return this;
    }

    #region Outbox
    public AwsConfigurator UseDynamoDbInbox()
    {
        _action += fluent => fluent.Subscriptions(s => s.UseDynamoDbInbox(_connection!));
        return this;
    }
    
    public AwsConfigurator UseDynamoDbInbox(string tableName)
    {
        _action += fluent => fluent.Subscriptions(x => x
            .UseDynamoDbInbox(cfg => cfg
                .SetConnection(_connection!)
                .SetTableName(tableName)));
        return this;
    }
    #endregion

    #region Outbox
    public AwsConfigurator UseDynamoDbOutbox()
    {
        _action += fluent => fluent.Producers(p => p.UseDynamoDbOutbox(_connection!));
        return this;
    }
    
    public AwsConfigurator UseDynamoDbOutbox(string tableName)
    {
        _action += fluent => fluent
            .Producers(p => p
                .UseDynamoDbOutbox(o => o
                    .SetConnection(_connection!)
                    .SetConfiguration(c => c.SetTableName(tableName))));
        return this;
    }
    
    public AwsConfigurator UseDynamoDbOutbox(Action<DynamoDbOutboxConfigurationBuilder> configure)
    {
        _action += fluent => fluent
            .Producers(p => p
                .UseDynamoDbOutbox(o => o
                    .SetConnection(_connection!)
                    .SetConfiguration(configure)));
        return this;
    }
    #endregion
    
    #region Distributed Lock

    public AwsConfigurator UseDynamoDbDistributedLock(Action<DynamoDbLockingProviderOptionsBuilder> configure)
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

    internal void SetFluentBrighter(FluentBrighterBuilder builder)
    {
        if (_connection == null)
        {
            throw new ConfigurationException("No MessagingGatewayConnection was set");
        }
        
        _action(builder);
    }
}