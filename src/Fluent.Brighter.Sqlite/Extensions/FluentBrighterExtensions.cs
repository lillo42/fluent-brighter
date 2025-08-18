using System;

using Fluent.Brighter.Sqlite;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring SQLite integration with Brighter's fluent builder
/// </summary>
public static class FluentBrighterExtensions
{
    /// <summary>
    /// Configures SQLite as the persistence store for Brighter's inbox and/or outbox
    /// </summary>
    /// <param name="builder">The Brighter fluent configuration builder</param>
    /// <param name="configure">Action to configure SQLite settings</param>
    /// <returns>The Brighter configuration builder for method chaining</returns>
    /// <remarks>
    /// This extension method enables centralized configuration of SQLite integration.
    /// Use the <see cref="SqliteConfigurator"/> within the configuration action to:
    /// <list type="bullet">
    /// <item><description>Set database connection details</description></item>
    /// <item><description>Enable the inbox feature</description></item>
    /// <item><description>Enable the outbox feature</description></item>
    /// </list>
    /// Example:
    /// <code>
    /// var builder = FluentBrighterBuilder
    ///     .New()
    ///     .UsingSqlite(cfg => cfg
    ///         .SetConnection(c => c.ConnectionString("Data Source=brighterdb.sqlite"))
    ///         .UseInbox()
    ///         .UseOutbox());
    /// </code>
    /// </remarks>
    public static FluentBrighterBuilder UsingSqlite(this FluentBrighterBuilder builder,
        Action<SqliteConfigurator> configure)
    {
        var configurator = new SqliteConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}