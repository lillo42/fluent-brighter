using Paramore.Brighter;
using Paramore.Brighter.Locking.MySql;
using Paramore.Brighter.MySql;

namespace Fluent.Brighter.MySql;

/// <summary>
/// A fluent builder for creating instances of <see cref="MySqlLockingProvider"/>.
/// Provides a clean, readable API for configuring inbox behavior including de-duplication, scope, and MySQL-specific settings.
/// </summary>
public class MySqlDistributedLockBuilder
{
    private RelationalDatabaseConfiguration? _configuration;
    
    /// <summary>
    /// Sets the MySQL-specific relational database configuration.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlDistributedLockBuilder Configuration(RelationalDatabaseConfiguration? configuration)
    {
        _configuration = configuration;
        return this;
    }
    
    /// <summary>
    /// Sets the MySQL-specific relational database configuration.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="RelationalDatabaseConfiguration"/>.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MySqlDistributedLockBuilder Configuration(Action<RelationalDatabaseConfigurationBuilder> configuration)
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
    public MySqlDistributedLockBuilder ConfigurationIfMissing(RelationalDatabaseConfiguration configuration)
    {
        _configuration ??= configuration;
        return this;
    }

    /// <summary>
    /// Create a new instance of <see cref="MySqlLockingProvider"/> based on provided information
    /// </summary>
    /// <returns>A new instance of <see cref="MySqlLockingProvider"/></returns>
    public MySqlLockingProvider Build()
    {
        return new MySqlLockingProvider(new MySqlConnectionProvider(_configuration));
    }
}