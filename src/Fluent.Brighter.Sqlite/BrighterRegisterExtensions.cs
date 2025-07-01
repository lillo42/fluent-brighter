using System;

namespace Fluent.Brighter.Sqlite;

/// <summary>
/// Provides extension methods for integrating SQLite-based messaging infrastructure with Brighter service configuration.
/// Enables fluent configuration of subscriptions, publications, outbox, inbox, and distributed locks using SQLite.
/// </summary>
/// <remarks>
/// This class simplifies the integration of SQLite as a messaging gateway and persistence store within Brighter's pipeline.
/// Use the <see cref="UsingSQLite(IBrighterConfigurator, Action{SqliteConfigurator})"/> method to define SQLite-specific settings.
/// </remarks>
public static class BrighterRegisterExtensions
{
    /// <summary>
    /// Configures SQLite integration for Brighter messaging using a fluent <see cref="SqliteConfigurator"/> setup.
    /// Applies the provided configuration to set up subscriptions, publications, and database connections.
    /// </summary>
    /// <param name="configurator">The Brighter configurator to extend.</param>
    /// <param name="configure">An action to customize the <see cref="SqliteConfigurator"/>.</param>
    /// <returns>The updated <see cref="IBrighterConfigurator"/> instance with SQLite services registered.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configurator"/> is null.</exception>
    /// <example>
    /// <code>
    /// services.AddBrighter(config =>
    /// {
    ///     config.UsingSQLite(pg =>
    ///     {
    ///         pg.Connection(builder => builder
    ///             .ConnectionString("my-db-connection-string")
    ///             );
    ///
    ///         pg.EnableUnitOfWork();
    ///         pg.UsingOutbox();
    ///         pg.UsingInbox();
    ///     });
    /// });
    /// </code>
    /// </example>
    public static IBrighterConfigurator UsingSQLite(this IBrighterConfigurator configurator, Action<SqliteConfigurator> configure)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        var postgresConfigurator = new SqliteConfigurator();
        configure(postgresConfigurator);
        return postgresConfigurator.AddMySql(configurator);
    }
}