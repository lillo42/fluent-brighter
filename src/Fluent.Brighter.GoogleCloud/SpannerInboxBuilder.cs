using Paramore.Brighter;
using Paramore.Brighter.Inbox.Spanner;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Builder class for creating instances of <see cref="SpannerInboxAsync"/>.
/// Provides a fluent API for configuring Google Cloud Spanner inbox storage.
/// </summary>
public sealed class SpannerInboxBuilder
{
    private IAmARelationalDatabaseConfiguration? _configuration;

    /// <summary>
    /// Sets the relational database configuration for the Spanner inbox.
    /// </summary>
    /// <param name="configuration">The database configuration containing connection settings and table names.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public SpannerInboxBuilder SetConfiguration(IAmARelationalDatabaseConfiguration configuration)
    {
        _configuration = configuration;
        return this;
    }

    private IAmARelationalDbConnectionProvider? _connectionProvider;

    /// <summary>
    /// Sets the relational database connection provider for the Spanner inbox.
    /// </summary>
    /// <param name="connectionProvider">The connection provider to use for managing database connections.</param>
    /// <returns>The builder instance for method chaining.</returns>
    public SpannerInboxBuilder SetConnectionProvider(IAmARelationalDbConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        return this;
    }
    
    /// <summary>
    /// Builds a new instance of <see cref="SpannerInboxAsync"/> using the configured settings.
    /// </summary>
    /// <returns>A configured <see cref="SpannerInboxAsync"/> instance.</returns>
    /// <exception cref="ConfigurationException">Thrown when configuration is not set.</exception>
    internal SpannerInboxAsync Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }
        
        return _connectionProvider == null ? new SpannerInboxAsync(_configuration) 
            : new SpannerInboxAsync(_configuration, _connectionProvider);
    }
}