using System;

namespace Fluent.Brighter.Postgres;

/// <summary>
/// Provides extension methods for integrating PostgreSQL-based messaging infrastructure with Brighter service configuration.
/// Enables fluent configuration of subscriptions, publications, outbox, inbox, and distributed locks using PostgreSQL.
/// </summary>
/// <remarks>
/// This class simplifies the integration of PostgreSQL as a messaging gateway and persistence store within Brighter's pipeline.
/// Use the <see cref="UsingPostgres(IBrighterConfigurator, Action{PostgresConfigurator})"/> method to define PostgreSQL-specific settings.
/// </remarks>
public static class BrighterRegisterExtensions
{
    /// <summary>
    /// Configures PostgreSQL integration for Brighter messaging using a fluent <see cref="PostgresConfigurator"/> setup.
    /// Applies the provided configuration to set up subscriptions, publications, and database connections.
    /// </summary>
    /// <param name="configurator">The Brighter configurator to extend.</param>
    /// <param name="configure">An action to customize the <see cref="PostgresConfigurator"/>.</param>
    /// <returns>The updated <see cref="IBrighterConfigurator"/> instance with PostgreSQL services registered.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configurator"/> is null.</exception>
    /// <example>
    /// <code>
    /// services.AddBrighter(config =>
    /// {
    ///     config.UsingPostgres(pg =>
    ///     {
    ///         pg.Configuration(builder => builder
    ///             .ConnectionString("my-db-connection-string")
    ///             .SchemaName("messaging"));
    ///
    ///         pg.Subscription(sub => sub
    ///             .MessageType<MyRequest>()
    ///             .ChannelName(new ChannelName("my-channel")));
    ///
    ///         pg.Publication(pub => pub
    ///             .Type("my-event-type")
    ///             .Topic(new RoutingKey("my-topic")));
    ///
    ///         pg.UseUnitOfWork();
    ///         pg.Outbox();
    ///         pg.Inbox();
    ///     });
    /// });
    /// </code>
    /// </example>
    public static IBrighterConfigurator UsingPostgres(this IBrighterConfigurator configurator, Action<PostgresConfigurator> configure)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        var postgresConfigurator = new PostgresConfigurator();
        configure(postgresConfigurator);
        return postgresConfigurator.AddPostgres(configurator);
    }
}