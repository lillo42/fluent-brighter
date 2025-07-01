namespace Fluent.Brighter.MySql;

/// <summary>
/// Provides extension methods for integrating MySQL-based messaging infrastructure with Brighter service configuration.
/// Enables fluent configuration of subscriptions, publications, outbox, inbox, and distributed locks using MySQL.
/// </summary>
/// <remarks>
/// This class simplifies the integration of MySQL as a messaging gateway and persistence store within Brighter's pipeline.
/// Use the <see cref="UsingMySQL(IBrighterConfigurator, Action{MySqlConfigurator})"/> method to define MySQL-specific settings.
/// </remarks>
public static class BrighterRegisterExtensions
{
    /// <summary>
    /// Configures MySQL integration for Brighter messaging using a fluent <see cref="MySqlConfigurator"/> setup.
    /// Applies the provided configuration to set up subscriptions, publications, and database connections.
    /// </summary>
    /// <param name="configurator">The Brighter configurator to extend.</param>
    /// <param name="configure">An action to customize the <see cref="MySqlConfigurator"/>.</param>
    /// <returns>The updated <see cref="IBrighterConfigurator"/> instance with MySQL services registered.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configurator"/> is null.</exception>
    /// <example>
    /// <code>
    /// services.AddBrighter(config =>
    /// {
    ///     config.Using(pg =>
    ///     {
    ///         pg.Configuration(builder => builder
    ///             .ConnectionString("my-db-connection-string")
    ///             .SchemaName("messaging"));
    ///
    ///         pg.UseUnitOfWork();
    ///         pg.Outbox();
    ///         pg.Inbox();
    ///     });
    /// });
    /// </code>
    /// </example>
    public static IBrighterConfigurator UsingMySQL(this IBrighterConfigurator configurator, Action<MySqlConfigurator> configure)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        var postgresConfigurator = new MySqlConfigurator();
        configure(postgresConfigurator);
        return postgresConfigurator.AddMySql(configurator);
    }
}