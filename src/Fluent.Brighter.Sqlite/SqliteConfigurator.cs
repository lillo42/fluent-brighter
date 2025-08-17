using System;

using Paramore.Brighter;

namespace Fluent.Brighter.Sqlite;

/// <summary>
/// Provides a fluent configuration interface for setting up SQLite integration with Brighter
/// </summary>
/// <remarks>
/// This configurator allows centralized setup of SQLite connections and Brighter features
/// like inbox and outbox. It builds a configuration action that will be applied later
/// during Brighter initialization.
/// </remarks>
public sealed class SqliteConfigurator
{
    private RelationalDatabaseConfiguration? _configuration;
    private Action<FluentBrighterBuilder> _action = _ => { };
    
    /// <summary>
    /// Configures the SQLite database connection using a fluent builder
    /// </summary>
    /// <param name="configuration">Action to configure SQLite database settings</param>
    /// <returns>This configurator for method chaining</returns>
    public SqliteConfigurator SetConnection(Action<RelationalDatabaseConfigurationBuilder> configuration)
    {
        var builder = new RelationalDatabaseConfigurationBuilder(); 
        configuration(builder);
        return SetConnection(builder.Build());
    }

    /// <summary>
    /// Sets the SQLite database connection using a pre-configured settings object
    /// </summary>
    /// <param name="configuration">Pre-configured SQLite database settings</param>
    /// <returns>This configurator for method chaining</returns>
    public SqliteConfigurator SetConnection(RelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    /// <summary>
    /// Enables and configures the SQLite inbox feature
    /// </summary>
    /// <returns>This configurator for method chaining</returns>
    /// <remarks>
    /// This method queues configuration to enable SQLite as the inbox store
    /// </remarks>
    public SqliteConfigurator UseInbox()
    {
        _action += fluent => fluent.Subscriptions(s => s.UseSqliteInbox(_configuration!));
        return this;
    }

    /// <summary>
    /// Enables and configures the SQLite outbox feature
    /// </summary>
    /// <returns>This configurator for method chaining</returns>
    /// <remarks>
    /// This method queues configuration to enable SQLite as the outbox store
    /// </remarks>
    public SqliteConfigurator UseOutbox()
    {
        _action += fluent => fluent.Producers(x => x.UseSqliteOutbox(_configuration!));
        return this;
    }

    /// <summary>
    /// Applies the accumulated configuration to Brighter's fluent builder
    /// </summary>
    /// <param name="fluentBrighter">Brighter's fluent configuration builder</param>
    /// <exception cref="ConfigurationException">
    /// Thrown if no database configuration was provided before applying
    /// </exception>
    internal void SetFluentBrighter(FluentBrighterBuilder fluentBrighter)
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("SQLite database configuration is missing. " +
                                             "You must set the connection using SetConnection() before enabling features. ");
        }
        
        _action(fluentBrighter);
    }
}