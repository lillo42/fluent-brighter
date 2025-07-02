using System;
using System.Data.Common;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;
using Paramore.Brighter.Sqlite;

namespace Fluent.Brighter.Sqlite;

/// <summary>
/// A fluent configuration builder for setting up SQLite integration with Paramore.Brighter message handling.
/// Provides methods to configure outbox, inbox and database connections.
/// 
/// </summary>
/// <remarks>
/// This class is designed to be used within a dependency injection pipeline to register SQLite-based infrastructure services.
/// It supports a fluent API for defining configuration steps and integrates with <see cref="IBrighterConfigurator"/> for service registration.
/// </remarks>
public class SqliteConfigurator
{
    private RelationalDatabaseConfiguration? _configuration;
    
    private bool _unitOfOWork;

    private SqliteOutboxBuilder? _outboxBuilder;
    private SqliteInboxBuilder? _inboxBuilder;

    /// <summary>
    /// Configures the relational database connection settings for SQLite.
    /// </summary>
    /// <param name="configure">An action to customize the <see cref="RelationalDatabaseConfigurationBuilder"/>.</param>
    /// <returns>The current <see cref="SqliteConfigurator"/> instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is null.</exception>
    public SqliteConfigurator Connection(Action<RelationalDatabaseConfigurationBuilder> configure)
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
    
    // <summary>
    /// Sets whether to use a unit of work.
    /// </summary>
    /// <param name="useUnitOfWork">True to enable unit of work; otherwise, false.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public SqliteConfigurator UseUnitOfWork(bool useUnitOfWork)
    {
        _unitOfOWork = useUnitOfWork;
        return this;
    }
    
    /// <summary>
    /// Enable to use a unit of work.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public SqliteConfigurator EnableUnitOfWork() => UseUnitOfWork(true);
    
    /// <summary>
    /// Disable to use a unit of work.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public SqliteConfigurator DisableUnitOfWork() => UseUnitOfWork(false); 
        
    /// <summary>
    /// Configures the outbox for message persistence.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="SqliteOutboxBuilder"/>.</param>
    /// <returns>The current <see cref="SqliteConfigurator"/> instance for fluent chaining.</returns>
    public SqliteConfigurator UsingOutbox(Action<SqliteOutboxBuilder>? configure = null)
    {
        _outboxBuilder = new SqliteOutboxBuilder();
        configure?.Invoke(_outboxBuilder);
        return this;
    }

    /// <summary>
    /// Configures the inbox for message de-duplication.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="SqliteInboxBuilder"/>.</param>
    /// <returns>The current <see cref="SqliteConfigurator"/> instance for fluent chaining.</returns>
    public SqliteConfigurator UsingInbox(Action<SqliteInboxBuilder>? configure = null)
    {
        _inboxBuilder = new SqliteInboxBuilder();
        configure?.Invoke(_inboxBuilder);
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
            provider = new SqliteUnitOfWork(_configuration);
            
            register.Services.AddSingleton(provider)
                .AddSingleton<IAmATransactionConnectionProvider>(sp => sp.GetRequiredService<SqliteUnitOfWork>())
                .AddSingleton<IAmARelationalDbConnectionProvider>(sp => sp.GetRequiredService<SqliteUnitOfWork>())
                .AddSingleton<IAmABoxTransactionProvider<DbTransaction>>(sp => sp.GetRequiredService<SqliteUnitOfWork>());
        }
        else
        {
            provider = new SqliteConnectionProvider(_configuration);
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

        return register;
    } 
}