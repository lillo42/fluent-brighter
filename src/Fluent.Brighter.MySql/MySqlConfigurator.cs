using System;

using Paramore.Brighter;

namespace Fluent.Brighter.MySql;

public sealed class MySqlConfigurator
{
    private RelationalDatabaseConfiguration? _configuration;
    private Action<FluentBrighterBuilder> _action = _ => { };
    
    public MySqlConfigurator SetConnection(Action<RelationalDatabaseConfigurationBuilder> configuration)
    {
        var builder = new RelationalDatabaseConfigurationBuilder(); 
        configuration(builder);
        return SetConnection(builder.Build());
    }

    public MySqlConfigurator SetConnection(RelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    public MySqlConfigurator UseInbox()
    {
        _action += fluent => fluent.Subscriptions(s => s.UseMySqlInbox(_configuration!));
        return this;
    }

    public MySqlConfigurator UseOutbox()
    {
        _action += fluent => fluent.Producers(x => x.UseMySqlOutbox(_configuration!));
        return this;
    }
    
    public MySqlConfigurator UseDistributedLock()
    {
        _action += fluent => fluent.Producers(x => x.UseMySqlDistributedLock(_configuration!));
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