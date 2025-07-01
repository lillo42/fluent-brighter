using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.MsSql;
using Paramore.Brighter.MsSql;

namespace Fluent.Brighter.MsSql;

/// <summary>
/// A fluent configuration builder for setting up MS SQL integration with Paramore.Brighter message handling.
/// Provides methods to configure subscriptions, publications, outbox, inbox, distributed locks, and database connections.
/// </summary>
/// <remarks>
/// This class is designed to be used within a dependency injection pipeline to register MS SQL-based infrastructure services.
/// It supports a fluent API for defining configuration steps and integrates with <see cref="IBrighterConfigurator"/> for service registration.
/// </remarks>
public class MsSqlConfigurator
{
    private readonly List<MsSqlSubscription> _subscriptions= [];
    private readonly List<Publication> _publications = [];
    private RelationalDatabaseConfiguration? _configuration;
    
    private bool _unitOfOWork;

    private MsSqlOutboxBuilder? _outboxBuilder;
    private MsSqlInboxBuilder? _inboxBuilder;
    private MsSqlDistributedLockBuilder? _distributedLockBuilder;

    /// <summary>
    /// Configures the relational database connection settings for MS SQL.
    /// </summary>
    /// <param name="configure">An action to customize the <see cref="RelationalDatabaseConfigurationBuilder"/>.</param>
    /// <returns>The current <see cref="MsSqlConfigurator"/> instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is null.</exception>
    public MsSqlConfigurator Connection(Action<RelationalDatabaseConfigurationBuilder> configure)
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
    
    /// <summary>
    /// Adds a MS SQL subscription configuration.
    /// </summary>
    /// <param name="configure">An action to customize the <see cref="MsSqlSubscriptionBuilder"/>.</param>
    /// <returns>The current <see cref="MsSqlConfigurator"/> instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is null.</exception>
    public MsSqlConfigurator AddSubscription(Action<MsSqlSubscriptionBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var builder = new MsSqlSubscriptionBuilder();
        configure(builder);
        _subscriptions.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Adds a MS SQL publication configuration.
    /// </summary>
    /// <param name="configure">An action to customize the <see cref="MsSqlPublicationBuilder"/>.</param>
    /// <returns>The current <see cref="MsSqlConfigurator"/> instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is null.</exception>
    public MsSqlConfigurator AddPublication(Action<MsSqlPublicationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var builder = new MsSqlPublicationBuilder();
        configure(builder);
        _publications.Add(builder.Build());
        return this;
    }

    /// <summary>
    /// Sets whether to use a unit of work.
    /// </summary>
    /// <param name="useUnitOfWork">True to enable unit of work; otherwise, false.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlConfigurator UseUnitOfWork(bool useUnitOfWork)
    {
        _unitOfOWork = useUnitOfWork;
        return this;
    }
    
    /// <summary>
    /// Enable to use a unit of work.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlConfigurator EnableUnitOfWork() => UseUnitOfWork(true);
    
    /// <summary>
    /// Disable to use a unit of work.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlConfigurator DisableUnitOfWork() => UseUnitOfWork(false);

    /// <summary>
    /// Configures the outbox for message persistence.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="MsSqlOutboxBuilder"/>.</param>
    /// <returns>The current <see cref="MsSqlConfigurator"/> instance for fluent chaining.</returns>
    public MsSqlConfigurator UsingOutbox(Action<MsSqlOutboxBuilder>? configure = null)
    {
        _outboxBuilder = new MsSqlOutboxBuilder();
        configure?.Invoke(_outboxBuilder);
        
        return this;
    }

    /// <summary>
    /// Configures the inbox for message de-duplication.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="MsSqlInboxBuilder"/>.</param>
    /// <returns>The current <see cref="MsSqlConfigurator"/> instance for fluent chaining.</returns>
    public MsSqlConfigurator UsingInbox(Action<MsSqlInboxBuilder>? configure = null)
    {
        _inboxBuilder = new MsSqlInboxBuilder();
        configure?.Invoke(_inboxBuilder);

        return this;
    }

    /// <summary>
    /// Configures distributed lock functionality using MS SQL advisory locks.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="MsSqlDistributedLockBuilder"/>.</param>
    /// <returns>The current <see cref="MsSqlConfigurator"/> instance for fluent chaining.</returns>
    public MsSqlConfigurator UsingDistributedLock(Action<MsSqlDistributedLockBuilder>? configure = null)
    {
        _distributedLockBuilder = new MsSqlDistributedLockBuilder();
        configure?.Invoke(_distributedLockBuilder);
        
        return this;
    }
    
    internal IBrighterConfigurator AddMsSql(IBrighterConfigurator register)
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("no connection setup");
        }
        
        IAmARelationalDbConnectionProvider? provider;
        if (_unitOfOWork)
        {
            provider = new MsSqlUnitOfWork(_configuration);
            
            register.Services.AddSingleton(provider)
                .AddSingleton<IAmATransactionConnectionProvider>(sp => sp.GetRequiredService<MsSqlUnitOfWork>())
                .AddSingleton<IAmARelationalDbConnectionProvider>(sp => sp.GetRequiredService<MsSqlUnitOfWork>())
                .AddSingleton<IAmABoxTransactionProvider<DbTransaction>>(sp => sp.GetRequiredService<MsSqlUnitOfWork>());
        }
        else
        {
            provider = new MsSqlConnectionProvider(_configuration);
        }

        if (_outboxBuilder != null)
        {
            register.Outbox(_outboxBuilder
                .SetConnectionIfIsMissing(_configuration)
                .UnitOfWorkConnectionProvider(provider)
                .Build());
        }

        if (_inboxBuilder != null)
        {
            register.Inbox(_inboxBuilder
                .SetConnectIfIsMissing(_configuration)
                .UnitOfWorkConnectionProvider(provider)
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
                .AddExternalBus(new MsSqlMessageProducerFactory(_configuration, _publications));
        } 
        
        if (_subscriptions.Count > 0)
        {
            _ = register
                .AddChannelFactory(
                    new ChannelFactory(new MsSqlMessageConsumerFactory(_configuration)), 
                    _subscriptions);
        }
        
        return register;
    } 
}