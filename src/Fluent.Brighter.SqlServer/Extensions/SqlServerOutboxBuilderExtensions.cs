using System;

namespace Fluent.Brighter.SqlServer.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="SqlServerOutboxBuilder"/> to enable fluent configuration
/// of the underlying SQL Server outbox settings.
/// </summary>
public static class SqlServerOutboxBuilderExtensions
{
    /// <summary>
    /// Configures the relational database settings for the SQL Server outbox using a fluent builder action.
    /// </summary>
    /// <param name="builder">The <see cref="SqlServerOutboxBuilder"/> instance to configure.</param>
    /// <param name="configure">An action that customizes the <see cref="RelationalDatabaseConfigurationBuilder"/> to define connection details and other database settings.</param>
    /// <returns>The updated <see cref="SqlServerOutboxBuilder"/> instance to allow method chaining.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="configure"/> is null.</exception>
    /// <remarks>
    /// This method simplifies configuration by allowing inline setup of database parameters (e.g., connection string, schema, table name)
    /// without requiring the caller to manually instantiate and build a configuration object.
    /// </remarks>
    public static SqlServerOutboxBuilder SetConfiguration(this SqlServerOutboxBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}