using System;

using Fluent.Brighter.MySql;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for integrating MySQL with Brighter's fluent configuration
/// </summary>
public static class FluentBrighterExtensions
{
    /// <summary>
    /// Configures MySQL as the persistence provider for Brighter features
    /// </summary>
    /// <param name="builder">The Brighter configuration builder</param>
    /// <param name="configure">Action to configure MySQL settings</param>
    /// <returns>The original builder for fluent chaining</returns>
    /// <remarks>
    /// <para>
    /// This is the entry point for MySQL configuration in Brighter. It enables:
    /// </para>
    /// <list type="bullet">
    /// <item><description>MySQL-based inbox</description></item>
    /// <item><description>MySQL-based outbox</description></item>
    /// <item><description>MySQL distributed locking</description></item>
    /// </list>
    /// <para>
    /// Example usage:
    /// </para>
    /// <code>
    /// BrighterHandlerBuilder
    ///     .Configure()
    ///     .UsingMySql(configurator => 
    ///     {
    ///         configurator
    ///             .SetConnection(cfg => { ... })
    ///             .UseInbox()
    ///             .UseOutbox()
    ///             .UseDistributedLock();
    ///     })
    ///     ./* other configuration */;
    /// </code>
    /// <para>
    /// This method delegates configuration to the <see cref="MySqlConfigurator"/> which provides
    /// a fluent interface for MySQL-specific settings.
    /// </para>
    /// </remarks>
    public static FluentBrighterBuilder UsingMySql(this FluentBrighterBuilder builder,
        Action<MySqlConfigurator> configure)
    {
        var configurator = new MySqlConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}