using System;

namespace Fluent.Brighter.Postgres;

/// <summary>
/// Provides extension methods for integrating Postgres with Brighter service configuration.
/// Enables fluent configuration of RabbitMQ-based messaging infrastructure.
/// </summary>
public static class BrighterRegisterExtensions
{
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