using System;

using Paramore.Brighter;

namespace Fluent.Brighter.Sqlite;

public sealed class SqliteConfigurator
{
    private RelationalDatabaseConfiguration? _configuration;
    private Action<FluentBrighterBuilder> _action = _ => { };
    
    public SqliteConfigurator SetConnection(Action<RelationalDatabaseConfigurationBuilder> configuration)
    {
        var builder = new RelationalDatabaseConfigurationBuilder(); 
        configuration(builder);
        return SetConnection(builder.Build());
    }

    public SqliteConfigurator SetConnection(RelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    public SqliteConfigurator UseInbox()
    {
        _action += fluent => fluent.Subscriptions(s => s.UseSqliteInbox(_configuration!));
        return this;
    }

    public SqliteConfigurator UseOutbox()
    {
        _action += fluent => fluent.Producers(x => x.UseSqliteOutbox(_configuration!));
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