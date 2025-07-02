using System;
using System.Data.Common;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;
using Paramore.Brighter.MySql;

namespace Fluent.Brighter.MySql;

/// <summary>
/// A fluent configuration builder for setting up MySQL integration with Paramore.Brighter message handling.
/// Provides methods to configure subscriptions, publications, outbox, inbox, distributed locks, and database connections.
/// </summary>
/// <remarks>
/// This class is designed to be used within a dependency injection pipeline to register MySQL-based infrastructure services.
/// It supports a fluent API for defining configuration steps and integrates with <see cref="IBrighterConfigurator"/> for service registration.
/// </remarks>
public class MySqlConfigurator
{
    private RelationalDatabaseConfiguration? _configuration;
    
    private bool _unitOfOWork;

    private MySqlOutboxBuilder? _outboxBuilder;
    private MySqlInboxBuilder? _inboxBuilder;
    private MySqlDistributedLockBuilder? _distributedLockBuilder;

    /// <summary>
    /// Configures the relational database connection settings for PostgreSQL.
    /// </summary>
    /// <param name="configure">An action to customize the <see cref="RelationalDatabaseConfigurationBuilder"/>.</param>
    /// <returns>The current <see cref="MySqlConfigurator"/> instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is null.</exception>
    public MySqlConfigurator Connection(Action<RelationalDatabaseConfigurationBuilder> configure)
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
    /// Sets whether to use a unit of work.
    /// </summary>
    /// <param name="useUnitOfWork">True to enable unit of work; otherwise, false.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlConfigurator UseUnitOfWork(bool useUnitOfWork)
    {
        _unitOfOWork = useUnitOfWork;
        return this;
    }
    
    /// <summary>
    /// Enable to use a unit of work.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlConfigurator EnableUnitOfWork() => UseUnitOfWork(true);
    
    /// <summary>
    /// Disable to use a unit of work.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlConfigurator DisableUnitOfWork() => UseUnitOfWork(false);
        
    /// <summary>
    /// Configures the outbox for message persistence.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="MySqlOutboxBuilder"/>.</param>
    /// <returns>The current <see cref="MySqlConfigurator"/> instance for fluent chaining.</returns>
    public MySqlConfigurator UsingOutbox(Action<MySqlOutboxBuilder>? configure = null)
    {
        _outboxBuilder = new MySqlOutboxBuilder();
        configure?.Invoke(_outboxBuilder);
        
        return this;
    }

    /// <summary>
    /// Configures the inbox for message de-duplication.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="MySqlInboxBuilder"/>.</param>
    /// <returns>The current <see cref="MySqlConfigurator"/> instance for fluent chaining.</returns>
    public MySqlConfigurator UsingInbox(Action<MySqlInboxBuilder>? configure = null)
    {
        _inboxBuilder = new MySqlInboxBuilder();
        configure?.Invoke(_inboxBuilder);

        return this;
    }

    /// <summary>
    /// Configures distributed lock functionality using MySQL locks.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="MySqlDistributedLockBuilder" />.</param>
    /// <returns>The current <see cref="MySqlConfigurator"/> instance for fluent chaining.</returns>
    public MySqlConfigurator UsingDistributedLock(Action<MySqlDistributedLockBuilder>? configure = null)
    {
        _distributedLockBuilder = new MySqlDistributedLockBuilder();
        configure?.Invoke(_distributedLockBuilder);
        
        return this;
    }
    
    internal IBrighterConfigurator AddMySql(IBrighterConfigurator register)
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("no connection setup");
        }
        
        IAmARelationalDbConnectionProvider? provider;
        if (_unitOfOWork)
        {
            provider = new MySqlUnitOfWork(_configuration);
            
            register.Services.AddSingleton(provider)
                .AddSingleton<IAmATransactionConnectionProvider>(sp => sp.GetRequiredService<MySqlUnitOfWork>())
                .AddSingleton<IAmARelationalDbConnectionProvider>(sp => sp.GetRequiredService<MySqlUnitOfWork>())
                .AddSingleton<IAmABoxTransactionProvider<DbTransaction>>(sp => sp.GetRequiredService<MySqlUnitOfWork>());
        }
        else
        {
            provider = new MySqlConnectionProvider(_configuration);
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
                .ConfigurationIfMissing(_configuration)
                .Build());
        }
        
        return register;
    } 
}