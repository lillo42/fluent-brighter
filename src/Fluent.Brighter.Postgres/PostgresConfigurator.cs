using System;
using System.Collections.Generic;
using System.Data.Common;

using Microsoft.Extensions.DependencyInjection;

using Npgsql;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;
using Paramore.Brighter.PostgreSql;

namespace Fluent.Brighter.Postgres;

/// <summary>
/// A fluent configuration builder for setting up PostgreSQL integration with Paramore.Brighter message handling.
/// Provides methods to configure subscriptions, publications, outbox, inbox, distributed locks, and database connections.
/// </summary>
/// <remarks>
/// This class is designed to be used within a dependency injection pipeline to register PostgreSQL-based infrastructure services.
/// It supports a fluent API for defining configuration steps and integrates with <see cref="IBrighterConfigurator"/> for service registration.
/// </remarks>
public class PostgresConfigurator
{
    private readonly List<PostgresSubscription> _subscriptions= [];
    private readonly List<PostgresPublication> _publications = [];
    private RelationalDatabaseConfiguration? _configuration;
    
    private bool _unitOfOWork;
    private NpgsqlDataSource? _dataSource;

    private PostgresOutboxBuilder? _outboxBuilder;
    private PostgresInboxBuilder? _inboxBuilder;
    private PostgresDistributedLockBuilder? _distributedLockBuilder;

    /// <summary>
    /// Configures the relational database connection settings for PostgreSQL.
    /// </summary>
    /// <param name="configure">An action to customize the <see cref="RelationalDatabaseConfigurationBuilder"/>.</param>
    /// <returns>The current <see cref="PostgresConfigurator"/> instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is null.</exception>
    public PostgresConfigurator Connection(Action<RelationalDatabaseConfigurationBuilder> configure)
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
    /// Adds a PostgreSQL subscription configuration.
    /// </summary>
    /// <param name="configure">An action to customize the <see cref="PostgresSubscriptionBuilder"/>.</param>
    /// <returns>The current <see cref="PostgresConfigurator"/> instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is null.</exception>
    public PostgresConfigurator AddSubscription(Action<PostgresSubscriptionBuilder> configure)
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

    /// <summary>
    /// Adds a PostgreSQL publication configuration.
    /// </summary>
    /// <param name="configure">An action to customize the <see cref="PostgresPublicationBuilder"/>.</param>
    /// <returns>The current <see cref="PostgresConfigurator"/> instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is null.</exception>
    public PostgresConfigurator AddPublication(Action<PostgresPublicationBuilder> configure)
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

    /// <summary>
    /// Sets whether to use a unit of work.
    /// </summary>
    /// <param name="useUnitOfWork">True to enable unit of work; otherwise, false.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresConfigurator UseUnitOfWork(bool useUnitOfWork)
    {
        _unitOfOWork = useUnitOfWork;
        return this;
    }
    
    /// <summary>
    /// Enable to use a unit of work.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresConfigurator EnableUnitOfWork() => UseUnitOfWork(true);
    
    /// <summary>
    /// Disable to use a unit of work.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresConfigurator DisableUnitOfWork() => UseUnitOfWork(false);

    /// <summary>
    /// Sets the Npgsql data source to be used for connection pooling.
    /// </summary>
    /// <param name="dataSource">The Npgsql data source instance.</param>
    /// <returns>The current <see cref="PostgresConfigurator"/> instance for fluent chaining.</returns>
    public PostgresConfigurator UseDataSource(NpgsqlDataSource? dataSource)
    {
        _dataSource = dataSource;
        return this;
    }
        
    /// <summary>
    /// Configures the outbox for message persistence.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="PostgresOutboxBuilder"/>.</param>
    /// <returns>The current <see cref="PostgresConfigurator"/> instance for fluent chaining.</returns>
    public PostgresConfigurator UsingOutbox(Action<PostgresOutboxBuilder>? configure = null)
    {
        _outboxBuilder = new PostgresOutboxBuilder();
        configure?.Invoke(_outboxBuilder);
        return this;
    }

    /// <summary>
    /// Configures the inbox for message de-duplication.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="PostgresInboxBuilder"/>.</param>
    /// <returns>The current <see cref="PostgresConfigurator"/> instance for fluent chaining.</returns>
    public PostgresConfigurator UsingInbox(Action<PostgresInboxBuilder>? configure = null)
    {
        _inboxBuilder = new PostgresInboxBuilder();
        configure?.Invoke(_inboxBuilder);

        return this;
    }

    /// <summary>
    /// Configures distributed lock functionality using PostgreSQL advisory locks.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="PostgresDistributedLockBuilder"/>.</param>
    /// <returns>The current <see cref="PostgresConfigurator"/> instance for fluent chaining.</returns>
    public PostgresConfigurator UsingDistributedLock(Action<PostgresDistributedLockBuilder>? configure = null)
    {
        _distributedLockBuilder = new PostgresDistributedLockBuilder();
        configure?.Invoke(_distributedLockBuilder);
        
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
            register.SetOutbox(_outboxBuilder
                .SetConnectionIfIsMissing(_configuration)
                .UnitOfWorkConnectionProvider(provider)
                .Build());
        }

        if (_inboxBuilder != null)
        {
            register.SetInbox(_inboxBuilder
                .SetConnectionIfIsMissing(_configuration)
                .UnitOfWorkConnectionProvider(provider)
                .Build());
        }

        if (_distributedLockBuilder != null)
        {
            register.SetDistributedLock(_distributedLockBuilder
                .SetConnectionIfMissing(_configuration)
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