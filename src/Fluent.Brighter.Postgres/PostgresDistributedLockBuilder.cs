using System;

using Paramore.Brighter;
using Paramore.Brighter.Locking.PostgresSql;

namespace Fluent.Brighter.Postgres;

/// <summary>
/// A fluent builder for creating instances of <see cref="PostgresLockingProvider"/>.
/// Provides a clean, readable API for configuring inbox behavior including de-duplication, scope, and PostgreSQL-specific settings.
/// </summary>
public class PostgresDistributedLockBuilder
{
    private RelationalDatabaseConfiguration? _configuration;
    
    /// <summary>
    /// Sets the PostgreSQL-specific relational database configuration.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresDistributedLockBuilder Connection(RelationalDatabaseConfiguration? configuration)
    {
        _configuration = configuration;
        return this;
    }
    
    /// <summary>
    /// Sets the PostgreSQL-specific relational database configuration.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresDistributedLockBuilder Connection(Action<RelationalDatabaseConfigurationBuilder> configuration)
    {
        var builder = new RelationalDatabaseConfigurationBuilder();
        configuration(builder);
        _configuration = builder.Build();
        return this;
    }

    /// <summary>
    /// Sets the PostgreSQL-specific relational database configuration if it was not set yet.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresDistributedLockBuilder SetConnectionIfMissing(RelationalDatabaseConfiguration configuration)
    {
        _configuration ??= configuration;
        return this;
    }

    /// <summary>
    /// Create a new instance of <see cref="PostgresLockingProvider"/> based on provided information
    /// </summary>
    /// <returns>A new instance of <see cref="PostgresLockingProvider"/></returns>
    public PostgresLockingProvider Build()
    {
        return new PostgresLockingProvider(new PostgresLockingProviderOptions(_configuration!.ConnectionString));
    }
}