using System;

using Fluent.Brighter.MySql;

namespace Fluent.Brighter;

/// <summary>
/// Provides fluent extension methods for configuring the <see cref="MySqlInboxBuilder"/>
/// </summary>
/// <remarks>
/// These extensions simplify MySQL inbox configuration using a delegate-based approach.
/// </remarks>
public static class MySqlInboxBuilderExtensions
{
    /// <summary>
    /// Configures the MySQL inbox settings using a fluent builder action
    /// </summary>
    /// <param name="builder">The inbox builder instance to configure</param>
    /// <param name="configure">Action that defines the database configuration settings</param>
    /// <returns>The original builder for fluent chaining</returns>
    public static MySqlInboxBuilder SetConfiguration(this MySqlInboxBuilder  builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}