using System;

namespace Fluent.Brighter.SqlServer.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="SqlServerInboxBuilder"/> to enable fluent configuration
/// of the underlying SQL Server inbox settings.
/// </summary>
public static class SqlServerInboxBuilderExtensions
{
    /// <summary>
    /// Configures the relational database settings for the SQL Server inbox using a fluent builder action.
    /// </summary>
    /// <param name="builder">The <see cref="SqlServerInboxBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that customizes the <see cref="RelationalDatabaseConfigurationBuilder"/> to define connection details, schema, table name, and other database settings.</param>
    /// <returns>The updated <see cref="SqlServerInboxBuilder"/> instance to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    /// <remarks>
    /// This method simplifies configuration by allowing inline setup of database parameters
    /// without requiring the caller to manually instantiate and build a configuration object.
    /// </remarks>
    public static SqlServerInboxBuilder SetConfiguration(this SqlServerInboxBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}