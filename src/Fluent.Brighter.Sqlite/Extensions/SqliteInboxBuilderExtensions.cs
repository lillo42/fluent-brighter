using System;

using Fluent.Brighter.Sqlite;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring SQLite inbox settings using a fluent builder pattern
/// </summary>
public static class SqliteInboxBuilderExtensions
{
    /// <summary>
    /// Configures the SQLite database settings using a fluent configuration builder
    /// </summary>
    /// <param name="builder">The SQLite inbox builder to extend</param>
    /// <param name="configure">Action to configure SQLite database settings</param>
    /// <returns>The SQLite inbox builder for method chaining</returns>
    /// <remarks>
    /// This extension method allows for fluent configuration of SQLite database settings
    /// through a delegate that acts on a <see cref="RelationalDatabaseConfigurationBuilder"/>
    /// </remarks>
    public static SqliteInboxBuilder SetConfiguration(this SqliteInboxBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}