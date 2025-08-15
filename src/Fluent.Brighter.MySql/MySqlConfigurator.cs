using System;

using Paramore.Brighter;

namespace Fluent.Brighter.MySql;

/// <summary>
/// A centralized configurator for MySQL integration in Brighter pipelines
/// </summary>
/// <remarks>
/// <para>
/// Provides a unified interface to configure MySQL connections and enable Brighter features:
/// </para>
/// <list type="bullet">
/// <item><description>Inbox (message consumption)</description></item>
/// <item><description>Outbox (message production)</description></item>
/// <item><description>Distributed locking (coordination)</description></item>
/// </list>
/// <para>
/// Usage typically follows the pattern:
/// </para>
/// <code>
/// var configurator = new MySqlConfigurator()
///     .SetConnection(cfg => { ... })
///     .UseInbox()
///     .UseOutbox()
///     .UseDistributedLock();
/// </code>
/// </remarks>
public sealed class MySqlConfigurator
{
    private RelationalDatabaseConfiguration? _configuration;
    private Action<FluentBrighterBuilder> _action = _ => { };
    
    /// <summary>
    /// Configures the MySQL connection using a fluent builder delegate
    /// </summary>
    /// <param name="configuration">Action that defines the database connection settings</param>
    /// <returns>This configurator for method chaining</returns>
    public MySqlConfigurator SetConnection(Action<RelationalDatabaseConfigurationBuilder> configuration)
    {
        var builder = new RelationalDatabaseConfigurationBuilder(); 
        configuration(builder);
        return SetConnection(builder.Build());
    }

    /// <summary>
    /// Sets the MySQL connection using a pre-configured database configuration
    /// </summary>
    /// <param name="configuration">The relational database configuration</param>
    /// <returns>This configurator for method chaining</returns>
    /// <remarks>
    /// Use this overload when you have an existing <see cref="RelationalDatabaseConfiguration"/> instance
    /// </remarks>
    public MySqlConfigurator SetConnection(RelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    /// <summary>
    /// Enables the MySQL inbox for message consumption
    /// </summary>
    /// <returns>This configurator for method chaining</returns>
    /// <remarks>
    /// Configures Brighter to use MySQL for:
    /// <list type="bullet">
    /// <item><description>Message deduplication</description></item>
    /// <item><description>Once-only message processing</description></item>
    /// <item><description>Message state tracking</description></item>
    /// </list>
    /// Requires prior connection configuration via <see cref="SetConnection(Action{RelationalDatabaseConfigurationBuilder})"/> or <see cref="SetConnection(RelationalDatabaseConfiguration)"/>.
    /// </remarks>
    public MySqlConfigurator UseInbox()
    {
        _action += fluent => fluent.Subscriptions(s => s.UseMySqlInbox(_configuration!));
        return this;
    }

    /// <summary>
    /// Enables the MySQL outbox for message production
    /// </summary>
    /// <returns>This configurator for method chaining</returns>
    /// <remarks>
    /// Configures Brighter to use MySQL for:
    /// <list type="bullet">
    /// <item><description>Transactional message storage</description></item>
    /// <item><description>Reliable message delivery</description></item>
    /// <item><description>Outbox pattern implementation</description></item>
    /// </list>
    /// Requires prior connection configuration.
    /// </remarks>
    public MySqlConfigurator UseOutbox()
    {
        _action += fluent => fluent.Producers(x => x.UseMySqlOutbox(_configuration!));
        return this;
    }
    
    /// <summary>
    /// Enables MySQL-based distributed locking
    /// </summary>
    /// <returns>This configurator for method chaining</returns>
    /// <remarks>
    /// Provides coordination for:
    /// <list type="bullet">
    /// <item><description>Producer message deduplication</description></item>
    /// <item><description>Resource contention management</description></item>
    /// <item><description>Distributed synchronization</description></item>
    /// </list>
    /// Requires prior connection configuration.
    /// </remarks>
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