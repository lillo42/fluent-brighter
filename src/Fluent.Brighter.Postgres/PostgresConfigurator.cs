using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter.Postgres;

public sealed class PostgresConfigurator
{
    private PostgresMessagingGatewayConnection? _connection;
    private Action<FluentBrighterBuilder> _action = _ => { };
    
    public PostgresConfigurator SetConnection(Action<RelationalDatabaseConfigurationBuilder> configuration)
    {
        var builder = new RelationalDatabaseConfigurationBuilder(); 
        configuration(builder);
        return SetConnection(builder.Build());
    }

    public PostgresConfigurator SetConnection(RelationalDatabaseConfiguration configuration)
    {
        _connection = new PostgresMessagingGatewayConnection(configuration);
        return this;
    }

    public PostgresConfigurator UseDistributedLock()
    {
        _action += fluent => fluent.Producers(x => x.UsePostgresDistributedLock(_connection!.Configuration));
        return this;
    }

    public PostgresConfigurator UseInbox()
    {
        _action += fluent => fluent.Subscriptions(s => s.UsePostgresInbox(_connection!.Configuration));
        return this;
    }

    public PostgresConfigurator UseOutbox()
    {
        _action += fluent => fluent.Producers(x => x.UsePostgresOutbox(_connection!.Configuration));
        return this;
    }

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


    internal void SetFluentBrighter(FluentBrighterBuilder fluentBrighter)
    {
        if (_connection == null)
        {
            throw new ConfigurationException("No RelationalDatabaseConfiguration was set");
        }
        
        _action(fluentBrighter);
    }
}