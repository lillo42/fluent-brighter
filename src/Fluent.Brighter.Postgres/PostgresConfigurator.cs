using System;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.Postgres;

namespace Fluent.Brighter.Postgres;

public sealed class PostgresConfigurator
{
    private RelationalDatabaseConfiguration? _configuration;
    private Action<FluentBrighterBuilder> _action = _ => { };
    
    public PostgresConfigurator SetConfiguration(Action<RelationalDatabaseConfigurationBuilder> configuration)
    {
        var builder = new RelationalDatabaseConfigurationBuilder(); 
        configuration(builder);
        return SetConfiguration(builder.Build());
    }

    public PostgresConfigurator SetConfiguration(RelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    public PostgresConfigurator UseDistributedLock()
    {
        _action += fluent => fluent.Producers(x => x.UsePostgresDistributedLock(_configuration!));
        return this;
    }

    public PostgresConfigurator UseInbox()
    {
        _action += fluent => fluent.Subscriptions(s => s.UsePostgresInbox(_configuration!));
        return this;
    }

    public PostgresConfigurator UseOutbox()
    {
        _action += fluent => fluent.Producers(x => x.UsePostgresOutbox(_configuration!));
        return this;
    }

    public PostgresConfigurator UsePublications(Action<PostgresMessageProducerFactoryBuilder> configure)
    {
        _action += fluent =>
        {
            fluent.Producers(producer => producer
                .AddPostgresPublication(cfg =>
                {
                    cfg.SetConnection(new PostgresMessagingGatewayConnection(_configuration!));
                    configure(cfg);
                }));
        };
        return this;
    }

    public PostgresConfigurator UseSubscriptions(Action<PostgresSubscriptionConfiguration> configure)
    {
        _action += fluent =>
        {
            fluent.Subscriptions(sub =>
            {
                var channel = new PostgresChannelFactory(new PostgresMessagingGatewayConnection(_configuration!));
                var configurator = new PostgresSubscriptionConfiguration(channel);
                configure(configurator);

                sub.AddPostgresChannelFactory(_configuration!);
                
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
        if (_configuration == null)
        {
            throw new ConfigurationException("No RelationalDatabaseConfiguration was set");
        }
        
        _action(fluentBrighter);
    }
}