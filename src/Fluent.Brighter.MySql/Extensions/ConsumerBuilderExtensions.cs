using System;

using Fluent.Brighter.MySql;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring MySQL inbox components in Brighter's consumer pipeline
/// </summary>
/// <remarks>
/// These fluent extensions simplify MySQL inbox configuration for message consumers.
/// </remarks>
public static class ConsumerBuilderExtensions
{
    /// <summary>
    /// Configures and enables the MySQL inbox using a fluent configuration delegate
    /// </summary>
    /// <param name="builder">The consumer builder instance</param>
    /// <param name="configure">Action that defines the MySQL inbox configuration</param>
    /// <returns>The consumer builder for fluent chaining</returns>
    public static ConsumerBuilder UseMySqlInbox(this ConsumerBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.UseMySqlInbox(configuration.Build());
    }

    /// <summary>
    /// Configures the MySQL inbox using a pre-built database configuration
    /// </summary>
    /// <param name="builder">The consumer builder instance</param>
    /// <param name="configuration">The relational database configuration</param>
    /// <returns>The consumer builder for fluent chaining</returns>
    /// <remarks>
    /// Use this overload when you have an existing <see cref="IAmARelationalDatabaseConfiguration"/> instance.
    /// </remarks>
    public static ConsumerBuilder UseMySqlInbox(this ConsumerBuilder builder, IAmARelationalDatabaseConfiguration configuration)
        => builder.UseMySqlInbox(cfg => cfg.SetConfiguration(configuration));

    /// <summary>
    /// Configures the MySQL inbox using a direct inbox builder delegate
    /// </summary>
    /// <param name="builder">The consumer builder instance</param>
    /// <param name="configure">Action that configures the MySQL inbox builder</param>
    /// <returns>The consumer builder for fluent chaining</returns>
    public static ConsumerBuilder UseMySqlInbox(this ConsumerBuilder builder, Action<MySqlInboxBuilder> configure)
    {
        var inbox = new MySqlInboxBuilder();
        configure(inbox);
        return builder.SetInbox(cfg => cfg.SetInbox(inbox.Build()));
    }
}