using System;

namespace Fluent.Brighter.MsSql;

/// <summary>
/// Provides extension methods for integrating MS SQL-based messaging infrastructure with Brighter service configuration.
/// Enables fluent configuration of subscriptions, publications, outbox, inbox, and distributed locks using MS SQL.
/// </summary>
/// <remarks>
/// This class simplifies the integration of MS SQL as a messaging gateway and persistence store within Brighter's pipeline.
/// Use the <see cref="UsingMsSql(IBrighterConfigurator, Action{MsSqlConfigurator})"/> method to define MS SQL-specific settings.
/// </remarks>
public static class BrighterRegisterExtensions
{
    /// <summary>
    /// Configures MS SQL integration for Brighter messaging using a fluent <see cref="MsSqlConfigurator"/> setup.
    /// Applies the provided configuration to set up subscriptions, publications, and database connections.
    /// </summary>
    /// <param name="configurator">The Brighter configurator to extend.</param>
    /// <param name="configure">An action to customize the <see cref="MsSqlConfigurator"/>.</param>
    /// <returns>The updated <see cref="IBrighterConfigurator"/> instance with MS SQL services registered.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configurator"/> is null.</exception>
    /// <example>
    /// <code>
    /// services.AddBrighter(config =>
    /// {
    ///     config.UsingMsSql(opt =>
    ///     {
    ///         opt.Connection(builder => builder
    ///             .ConnectionString("my-db-connection-string")
    ///             );
    ///
    ///         opt.AddSubscription(sub => sub
    ///             .MessageType<MyRequest>()
    ///             .ChannelName(new ChannelName("my-channel")));
    ///
    ///         opt.AddPublication(pub => pub
    ///             .Type("my-event-type")
    ///             .Queue(new RoutingKey("my-topic")));
    ///
    ///         opt.EnableUnitOfWork();
    ///         opt.UsingOutbox();
    ///         opt.UsingInbox();
    ///         opt.UsingDistributedLock();
    ///     });
    /// });
    /// </code>
    /// </example>
    public static IBrighterConfigurator UsingMsSql(this IBrighterConfigurator configurator, Action<MsSqlConfigurator> configure)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        var postgresConfigurator = new MsSqlConfigurator();
        configure(postgresConfigurator);
        return postgresConfigurator.AddMsSql(configurator);
    }
}