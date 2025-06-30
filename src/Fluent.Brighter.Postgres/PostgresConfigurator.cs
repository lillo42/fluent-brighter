using System;
using System.Collections.Generic;
using System.Data.Common;

using Microsoft.Extensions.DependencyInjection;

using Npgsql;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;
using Paramore.Brighter.PostgreSql;

namespace Fluent.Brighter.Postgres;

public class PostgresConfigurator
{
    private readonly List<PostgresSubscription> _subscriptions= [];
    private readonly List<PostgresPublication> _publications = [];
    private RelationalDatabaseConfiguration? _configuration;
    
    private bool _unitOfOWork;
    private NpgsqlDataSource? _dataSource;

    private PostgresOutboxBuilder? _outboxBuilder;
    private PostgresInboxConfigurationBuilder? _inboxBuilder;
    private PostgresDistributedLockBuilder? _distributedLockBuilder;

    public PostgresConfigurator Configuration(Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var builder = new RelationalDatabaseConfigurationBuilder();
        configure(builder);
        _configuration = builder.Build();
        return this;
    }
    
    public PostgresConfigurator Subscription(Action<PostgresSubscriptionBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var builder = new PostgresSubscriptionBuilder();
        configure(builder);
        _subscriptions.Add(builder.Build());
        return this;
    }

    public PostgresConfigurator Publication(Action<PostgresPublicationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var builder = new PostgresPublicationBuilder();
        configure(builder);
        _publications.Add(builder.Build());
        return this;
    }

    public PostgresConfigurator UseUnitOfWork()
    {
        _unitOfOWork = true;
        return this;
    }

    public PostgresConfigurator UseDataSource(NpgsqlDataSource? dataSource)
    {
        _dataSource = dataSource;
        return this;
    }
        
    public PostgresConfigurator Outbox(Action<PostgresOutboxBuilder>? configure = null)
    {
        _outboxBuilder = new PostgresOutboxBuilder();
        if (configure != null)
        {
            configure(_outboxBuilder);
        }
        
        return this;
    }

    public PostgresConfigurator Inbox(Action<PostgresInboxConfigurationBuilder>? configure = null)
    {
        _inboxBuilder = new PostgresInboxConfigurationBuilder();
        if (configure != null)
        {
            configure(_inboxBuilder);
        }

        return this;
    }

    public PostgresConfigurator DistributedLock(Action<PostgresDistributedLockBuilder>? configure = null)
    {
        _distributedLockBuilder = new PostgresDistributedLockBuilder();
        if (configure != null)
        {
            configure(_distributedLockBuilder);
        }
        
        return this;
    }
    
    internal IBrighterConfigurator AddPostgres(IBrighterConfigurator register)
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("no connection setup");
        }
        
        IAmARelationalDbConnectionProvider? provider;
        if (_unitOfOWork)
        {
            provider = new PostgreSqlUnitOfWork(_configuration, _dataSource);
            
            register.Services.AddSingleton(provider)
                .AddSingleton<IAmATransactionConnectionProvider>(sp => sp.GetRequiredService<PostgreSqlUnitOfWork>())
                .AddSingleton<IAmARelationalDbConnectionProvider>(sp => sp.GetRequiredService<PostgreSqlUnitOfWork>())
                .AddSingleton<IAmABoxTransactionProvider<DbTransaction>>(sp => sp.GetRequiredService<PostgreSqlUnitOfWork>());
        }
        else
        {
            provider = new PostgreSqlConnectionProvider(_configuration, _dataSource);
        }

        if (_outboxBuilder != null)
        {
            register.Outbox(_outboxBuilder
                .ConfigurationIfIsMissing(_configuration)
                .SetProvider(provider)
                .Build());
        }

        if (_inboxBuilder != null)
        {
            register.Inbox(_inboxBuilder
                .ConfigurationIfIsMissing(_configuration)
                .SetProvider(provider)
                .Build());
        }

        if (_distributedLockBuilder != null)
        {
            register.DistributedLock(_distributedLockBuilder
                .ConfigurationIfMissing(_configuration)
                .Build());
        }
        
        if (_publications.Count > 0)
        {
            _ = register
                .AddExternalBus(new PostgresMessageProducerFactory(
                    new PostgresMessagingGatewayConnection(_configuration), _publications));
        } 
        
        if (_subscriptions.Count > 0)
        {
            _ = register
                .AddChannelFactory(
                    new PostgresChannelFactory(new PostgresMessagingGatewayConnection(_configuration)), 
                    _subscriptions);
        }
        
        return register;
    } 
}