using System;

using Fluent.Brighter.MySql;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring <see cref="MySqlOutboxBuilder"/> using a fluent interface
/// </summary>
public static class MySqlOutboxBuilderExtensions
{
    /// <summary>
    /// Configures the MySQL outbox settings using a fluent builder action
    /// </summary>
    /// <param name="builder">The outbox builder instance</param>
    /// <param name="configure">Configuration action that setups the database settings</param>
    /// <returns>The original builder for method chaining</returns>
    public static MySqlOutboxBuilder SetConfiguration(this MySqlOutboxBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}