using System;
using System.Data.Common;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;
using Paramore.Brighter.Sqlite;

namespace Fluent.Brighter.Sqlite;

/// <summary>
/// A fluent configuration builder for setting up SQLite integration with Paramore.Brighter message handling.
/// Provides methods to configure subscriptions, publications, outbox, inbox, distributed locks, and database connections.
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
    /// Configures the relational database connection settings for PostgreSQL.
    /// </summary>
    /// <param name="configure">An action to customize the <see cref="RelationalDatabaseConfigurationBuilder"/>.</param>
    /// <returns>The current <see cref="SqliteConfigurator"/> instance for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configure"/> is null.</exception>
    public SqliteConfigurator Configuration(Action<RelationalDatabaseConfigurationBuilder> configure)
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
    /// Enables unit of work support for transactional message handling.
    /// </summary>
    /// <returns>The current <see cref="SqliteConfigurator"/> instance for fluent chaining.</returns>
    public SqliteConfigurator UseUnitOfWork()
    {
        _unitOfOWork = true;
        return this;
    }
        
    /// <summary>
    /// Configures the outbox for message persistence.
    /// </summary>
    /// <param name="configure">An optional action to customize the <see cref="SqliteOutboxBuilder"/>.</param>
    /// <returns>The current <see cref="SqliteConfigurator"/> instance for fluent chaining.</returns>
    public SqliteConfigurator Outbox(Action<SqliteOutboxBuilder>? configure = null)
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
    public SqliteConfigurator Inbox(Action<SqliteInboxBuilder>? configure = null)
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
            register.Outbox(_outboxBuilder
                .ConfigurationIfIsMissing(_configuration)
                .UnitOfWorkConnectionProvider(provider)
                .Build());
        }

        if (_inboxBuilder != null)
        {
            register.Inbox(_inboxBuilder
                .ConfigurationIfIsMissing(_configuration)
                .UnitOfWorkConnectionProvider(provider)
                .Build());
        }

        return register;
    } 
}