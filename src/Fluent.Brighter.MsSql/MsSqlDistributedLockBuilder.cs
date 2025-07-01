using System;

using Paramore.Brighter;
using Paramore.Brighter.Locking.MsSql;
using Paramore.Brighter.MsSql;

namespace Fluent.Brighter.MsSql;

/// <summary>
/// A fluent builder for creating instances of <see cref="MsSqlLockingProvider"/>.
/// Provides a clean, readable API for configuring inbox behavior including de-duplication, scope, and MS SQL-specific settings.
/// </summary>
public class MsSqlDistributedLockBuilder
{
    private RelationalDatabaseConfiguration? _configuration;
    
    /// <summary>
    /// Sets the MySQL-specific relational database configuration.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlDistributedLockBuilder Connection(RelationalDatabaseConfiguration? configuration)
    {
        _configuration = configuration;
        return this;
    }
    
    /// <summary>
    /// Sets the MySQL-specific relational database configuration.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlDistributedLockBuilder Connection(Action<RelationalDatabaseConfigurationBuilder> configuration)
    {
        var builder = new RelationalDatabaseConfigurationBuilder();
        configuration(builder);
        _configuration = builder.Build();
        return this;
    }

    /// <summary>
    /// Sets the MySQL-specific relational database configuration if it was not set yet.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MsSqlDistributedLockBuilder ConfigurationIfMissing(RelationalDatabaseConfiguration configuration)
    {
        _configuration ??= configuration;
        return this;
    }

    /// <summary>
    /// Create a new instance of <see cref="MsSqlLockingProvider"/> based on provided information
    /// </summary>
    /// <returns>A new instance of <see cref="MsSqlLockingProvider"/></returns>
    public MsSqlLockingProvider Build()
    {
        return new MsSqlLockingProvider(new MsSqlConnectionProvider(_configuration));
    }
}