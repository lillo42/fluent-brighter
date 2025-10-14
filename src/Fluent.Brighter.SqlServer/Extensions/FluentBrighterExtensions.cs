using System;

using Fluent.Brighter.SqlServer;

namespace Fluent.Brighter;


public static class FluentBrighterExtensions
{
    /// <summary>
    /// Configures Fluent Brighter to use Microsoft SQL Server for core messaging patterns—including
    /// inbox, outbox, distributed locking, publications, and subscriptions—via a fluent, centralized configurator.
    /// </summary>
    /// <param name="builder">The Fluent Brighter builder to extend.</param>
    /// <param name="configure">An action that customizes the <see cref="SqlServerConfigurator"/> to define SQL Server integration settings.</param>
    /// <returns>The updated <see cref="FluentBrighterBuilder"/> to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    /// <remarks>
    /// The <see cref="SqlServerConfigurator"/> shares a single database connection across all enabled features
    /// (e.g., inbox, outbox, locking) unless overridden per-feature.
    /// </remarks>
    public static FluentBrighterBuilder UsingMicrosoftSqlServer(this FluentBrighterBuilder builder,
        Action<SqlServerConfigurator> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var configurator = new SqlServerConfigurator();
        configure(configurator);
        configurator.SetFluentBrighter(builder);
        return builder;
    }
}