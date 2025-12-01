using Paramore.Brighter;
using Paramore.Brighter.Outbox.Spanner;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Builder class for creating instances of <see cref="SpannerOutbox"/>.
/// Provides a fluent API for configuring Google Cloud Spanner outbox storage.
/// </summary>
public sealed class SpannerOutboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    /// <summary>
    /// Sets the relational database configuration for the Spanner outbox.
    /// </summary>
    /// <param name="configuration">The database configuration containing connection settings and table names.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public SpannerOutboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmARelationalDbConnectionProvider? _connectionProvider;

    /// <summary>
    /// Sets the relational database connection provider for the Spanner outbox.
    /// </summary>
    /// <param name="connectionProvider">The connection provider to use for managing database connections.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public SpannerOutboxBuilder SetConnectionProvider(IAmARelationalDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    /// <summary>
    /// Builds a new instance of <see cref="SpannerOutbox"/> using the configured settings.
    /// </summary>
    /// <returns>A configured <see cref="SpannerOutbox"/> instance.</returns>
    /// <exception cref="ConfigurationException">Thrown when configuration is not set.</exception>
    internal SpannerOutbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }
        
        return _connectionProvider == null ? new SpannerOutbox(_configuration) 
            : new SpannerOutbox(_configuration, _connectionProvider);
    }
}