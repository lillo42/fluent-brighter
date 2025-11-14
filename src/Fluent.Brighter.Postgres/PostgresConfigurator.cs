using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter.Postgres;

/// <summary>
/// Provides a fluent API for configuring PostgreSQL integration with Fluent Brighter.
/// This configurator allows you to set up PostgreSQL-based messaging, outbox patterns, inbox patterns,
/// distributed locking, and message subscriptions/publications.
/// </summary>
public sealed class PostgresConfigurator
{
    private PostgresMessagingGatewayConnection? _connection;
    private Action<FluentBrighterBuilder> _action = _ => { };
    
    /// <summary>
    /// Sets the PostgreSQL database connection using a fluent configuration builder.
    /// </summary>
    /// <param name="configuration">An action that configures the relational database connection settings.</param>
    /// <returns>The current <see cref="PostgresConfigurator"/> instance for method chaining.</returns>
    public PostgresConfigurator SetConnection(Action<RelationalDatabaseConfigurationBuilder> configuration)
    {
        var builder = new RelationalDatabaseConfigurationBuilder(); 
        configuration(builder);
        return SetConnection(builder.Build());
    }

    /// <summary>
    /// Sets the PostgreSQL database connection using a pre-configured <see cref="RelationalDatabaseConfiguration"/> object.
    /// </summary>
    /// <param name="configuration">The relational database configuration containing connection details.</param>
    /// <returns>The current <see cref="PostgresConfigurator"/> instance for method chaining.</returns>
    public PostgresConfigurator SetConnection(RelationalDatabaseConfiguration configuration)
    {
        _connection = new PostgresMessagingGatewayConnection(configuration);
        return this;
    }

    /// <summary>
    /// Enables PostgreSQL-based distributed locking for message producers.
    /// This ensures that only one producer instance can send messages at a time in a distributed environment.
    /// </summary>
    /// <returns>The current <see cref="PostgresConfigurator"/> instance for method chaining.</returns>
    public PostgresConfigurator UseDistributedLock()
    {
        _action += fluent => fluent.Producers(x => x.UsePostgresDistributedLock(_connection!.Configuration));
        return this;
    }

    /// <summary>
    /// Enables PostgreSQL-based inbox pattern for subscriptions.
    /// The inbox pattern ensures message deduplication and idempotent message processing by storing
    /// information about received messages in PostgreSQL.
    /// </summary>
    /// <returns>The current <see cref="PostgresConfigurator"/> instance for method chaining.</returns>
    public PostgresConfigurator UseInbox()
    {
        _action += fluent => fluent.Subscriptions(s => s.UsePostgresInbox(_connection!.Configuration));
        return this;
    }

    /// <summary>
    /// Enables PostgreSQL-based outbox pattern for message producers.
    /// The outbox pattern ensures reliable message publishing by storing outgoing messages in PostgreSQL
    /// as part of the same transaction as domain changes, guaranteeing eventual consistency.
    /// </summary>
    /// <returns>The current <see cref="PostgresConfigurator"/> instance for method chaining.</returns>
    public PostgresConfigurator UseOutbox()
    {
        _action += fluent => fluent.Producers(x => x.UsePostgresOutbox(_connection!.Configuration));
        return this;
    }

    /// <summary>
    /// Configures PostgreSQL-based message publications.
    /// This method allows you to define how messages are published through PostgreSQL messaging gateway,
    /// including topic mappings and producer settings.
    /// </summary>
    /// <param name="configure">An action that configures the PostgreSQL message producer factory.</param>
    /// <returns>The current <see cref="PostgresConfigurator"/> instance for method chaining.</returns>
    public PostgresConfigurator UsePublications(Action<PostgresMessageProducerFactoryBuilder> configure)
    {
        _action += fluent =>
        {
            fluent.Producers(producer => producer
                .AddPostgresPublication(cfg =>
                {
                    cfg.SetConnection(_connection!);
                    configure(cfg);
                }));
        };
        return this;
    }

    /// <summary>
    /// Configures PostgreSQL-based message subscriptions.
    /// This method allows you to define how messages are consumed from PostgreSQL messaging gateway,
    /// including channel configurations and subscription mappings for different message types.
    /// </summary>
    /// <param name="configure">An action that configures the PostgreSQL subscriptions using the <see cref="PostgresSubscriptionConfigurator"/>.</param>
    /// <returns>The current <see cref="PostgresConfigurator"/> instance for method chaining.</returns>
    public PostgresConfigurator UseSubscriptions(Action<PostgresSubscriptionConfigurator> configure)
    {
        _action += fluent =>
        {
            fluent.Subscriptions(sub =>
            {
                var channel = new PostgresChannelFactory(_connection!);
                var configurator = new PostgresSubscriptionConfigurator(channel);
                configure(configurator);

                sub.AddChannelFactory(channel);
                
                foreach (var subscription in configurator.Subscriptions)
                {
                    sub.AddPostgresSubscription(subscription);
                }
            });
        };
        return this;
    }

    /// <summary>
    /// Applies the configured PostgreSQL settings to the Fluent Brighter builder.
    /// This method is called internally to set up all the configured PostgreSQL features.
    /// </summary>
    /// <param name="fluentBrighter">The Fluent Brighter builder instance to configure.</param>
    /// <exception cref="ConfigurationException">Thrown when no database connection has been configured via <see cref="SetConnection(Action{RelationalDatabaseConfigurationBuilder})"/> or <see cref="SetConnection(RelationalDatabaseConfiguration)"/>.</exception>
    internal void SetFluentBrighter(FluentBrighterBuilder fluentBrighter)
    {
        if (_connection == null)
        {
            throw new ConfigurationException("No RelationalDatabaseConfiguration was set");
        }
        
        _action(fluentBrighter);
    }
}