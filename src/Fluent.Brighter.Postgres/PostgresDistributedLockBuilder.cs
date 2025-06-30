using System;

using Paramore.Brighter;
using Paramore.Brighter.Locking.PostgresSql;

namespace Fluent.Brighter.Postgres;

public class PostgresDistributedLockBuilder
{
    private RelationalDatabaseConfiguration? _configuration;
    
    /// <summary>
    /// Sets the PostgreSQL-specific relational database configuration.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresDistributedLockBuilder Configuration(RelationalDatabaseConfiguration? configuration)
    {
        _configuration = configuration;
        return this;
    }
    
    /// <summary>
    /// Sets the PostgreSQL-specific relational database configuration.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public PostgresDistributedLockBuilder Configuration(Action<RelationalDatabaseConfigurationBuilder> configuration)
    {
        var builder = new RelationalDatabaseConfigurationBuilder();
        configuration(builder);
        _configuration = builder.Build();
        return this;
    }


    internal PostgresDistributedLockBuilder ConfigurationIfMissing(RelationalDatabaseConfiguration configuration)
    {
        _configuration ??= configuration;
        return this;
    }


    internal PostgresLockingProvider Build()
    {
        return new PostgresLockingProvider(new PostgresLockingProviderOptions(_configuration!.ConnectionString));
    }
}