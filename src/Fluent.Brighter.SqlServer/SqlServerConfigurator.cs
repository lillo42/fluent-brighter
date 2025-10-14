using System;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.MsSql;

namespace Fluent.Brighter.SqlServer;

/// <summary>
/// Provides a high-level, fluent configurator for integrating Microsoft SQL Server features
/// into a Brighter application using the Fluent Brighter library.
/// </summary>
/// <remarks>
/// This class simplifies the setup of common SQL Server-based messaging patterns—including
/// inbox, outbox, distributed locking, publications, and subscriptions—by sharing a common
/// database configuration. It is part of the Fluent Brighter library
/// (<see href="https://github.com/lillo42/fluent-brighter/"/>), which extends Paramore.Brighter
/// with fluent APIs and relational database support.
/// </remarks>
public sealed class SqlServerConfigurator
{
    private RelationalDatabaseConfiguration? _configuration;
    private Action<FluentBrighterBuilder> _action = _ => { };

    /// <summary>
    /// Sets the SQL Server connection configuration using a fluent builder action.
    /// </summary>
    /// <param name="configuration">An action that configures the <see cref="RelationalDatabaseConfigurationBuilder"/>.</param>
    /// <returns>The current instance of <see cref="SqlServerConfigurator"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
    public SqlServerConfigurator SetConnection(Action<RelationalDatabaseConfigurationBuilder> configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        
        var builder = new RelationalDatabaseConfigurationBuilder();
        configuration(builder);
        return SetConnection(builder.Build());
    }

    /// <summary>
    /// Sets the SQL Server connection configuration explicitly.
    /// </summary>
    /// <param name="configuration">The relational database configuration containing connection details.</param>
    /// <returns>The current instance of <see cref="SqlServerConfigurator"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configuration"/> is null.</exception>
    public SqlServerConfigurator SetConnection(RelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        return this;
    }

    /// <summary>
    /// Enables distributed locking using SQL Server to coordinate singleton or exclusive message processing.
    /// Requires that a connection has been configured via <see cref="SetConnection"/>.
    /// </summary>
    /// <returns>The current instance of <see cref="SqlServerConfigurator"/> to allow method chaining.</returns>
    public SqlServerConfigurator UseDistributedLock()
    {
        _action += fluent => fluent.Producers(x => x.UseMicrosoftSqlServerDistributedLock(_configuration!));
        return this;
    }

    /// <summary>
    /// Enables the inbox pattern using SQL Server for message deduplication.
    /// Uses the shared connection configured via <see cref="SetConnection"/>.
    /// </summary>
    /// <returns>The current instance of <see cref="SqlServerConfigurator"/> to allow method chaining.</returns>
    public SqlServerConfigurator UseInbox()
    {
        _action += fluent => fluent.Subscriptions(s => s.UseMicrosoftSqlServerInbox(_configuration!));
        return this;
    }

    /// <summary>
    /// Enables the inbox pattern with additional custom database configuration (e.g., custom schema or table name),
    /// while inheriting the base connection string from the shared configuration.
    /// </summary>
    /// <param name="configure">An action to further customize the inbox database settings.</param>
    /// <returns>The current instance of <see cref="SqlServerConfigurator"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public SqlServerConfigurator UseInbox(Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        _action += fluent => fluent.Subscriptions(s => s.UseMicrosoftSqlServerInbox(c =>
        {
            c.SetConnectionString(_configuration!.ConnectionString);
            configure(c);
        }));
        return this;
    }

    /// <summary>
    /// Enables the outbox pattern using SQL Server for reliable, transactional message publishing.
    /// Uses the shared connection configured via <see cref="SetConnection"/>.
    /// </summary>
    /// <returns>The current instance of <see cref="SqlServerConfigurator"/> to allow method chaining.</returns>
    public SqlServerConfigurator UseOutbox()
    {
        _action += fluent => fluent.Producers(x => x.UseMicrosoftSqlServerOutbox(_configuration!));
        return this;
    }

    /// <summary>
    /// Enables the outbox pattern with additional custom database configuration (e.g., custom outbox table name),
    /// while inheriting the base connection string from the shared configuration.
    /// </summary>
    /// <param name="configure">An action to further customize the outbox database settings.</param>
    /// <returns>The current instance of <see cref="SqlServerConfigurator"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public SqlServerConfigurator UseOutbox(Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        _action += fluent => fluent.Producers(s => s.UseMicrosoftSqlServerOutbox(c =>
        {
            c.SetConnectionString(_configuration!.ConnectionString);
            configure(c);
        }));
        return this;
    }

    /// <summary>
    /// Configures one or more message publications that persist to SQL Server via the outbox.
    /// The shared connection is automatically applied to the producer factory.
    /// </summary>
    /// <param name="configure">An action that configures the <see cref="SqlServerMessageProducerFactoryBuilder"/>.</param>
    /// <returns>The current instance of <see cref="SqlServerConfigurator"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public SqlServerConfigurator UsePublications(Action<SqlServerMessageProducerFactoryBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        _action += fluent =>
        {
            fluent.Producers(producer => producer
                .AddMicrosoftSqlServerPublication(cfg =>
                {
                    cfg.SetConnection(_configuration!);
                    configure(cfg);
                }));
        };
        return this;
    }

    /// <summary>
    /// Configures message subscriptions that consume from SQL Server-backed channels.
    /// A dedicated <see cref="ChannelFactory"/> using <see cref="MsSqlMessageConsumerFactory"/>
    /// is created and registered automatically.
    /// </summary>
    /// <param name="configure">An action that configures a <see cref="SqlServerSubscriptionConfigurator"/>.</param>
    /// <returns>The current instance of <see cref="SqlServerConfigurator"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    public SqlServerConfigurator UseSubscriptions(Action<SqlServerSubscriptionConfigurator> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        _action += fluent =>
        {
            fluent.Subscriptions(sub =>
            {
                var channel = new ChannelFactory(new MsSqlMessageConsumerFactory(_configuration!));
                var configurator = new SqlServerSubscriptionConfigurator(channel);
                configure(configurator);

                sub.AddChannelFactory(channel);

                foreach (var subscription in configurator.Subscriptions)
                {
                    sub.AddMicrosoftSqlServerSubscription(subscription);
                }
            });
        };
        return this;
    }

    /// <summary>
    /// Applies all configured SQL Server features to the provided <see cref="FluentBrighterBuilder"/>.
    /// </summary>
    /// <param name="fluentBrighter">The Fluent Brighter builder to configure.</param>
    /// <exception cref="ConfigurationException">Thrown if no database configuration was set via <see cref="SetConnection"/>.</exception>
    internal void SetFluentBrighter(FluentBrighterBuilder fluentBrighter)
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("No RelationalDatabaseConfiguration was set");
        }

        _action(fluentBrighter);
        fluentBrighter.RegisterServices(service => service
            .AddSingleton(_configuration)
            .AddSingleton<IAmARelationalDatabaseConfiguration>(_configuration));
    }
}