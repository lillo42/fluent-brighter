using Paramore.Brighter;
using Paramore.Brighter.Inbox.Spanner;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Builder class for creating instances of <see cref="SpannerInbox"/>.
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
    public SpannerInboxBuilder SetConnectionProvider(
        IAmARelationalDbConnectionProvider connectionProvider
    )
    {
        _connectionProvider = connectionProvider;
        return this;
    }

    /// <summary>
    /// Builds a new instance of <see cref="SpannerInbox"/> using the configured settings.
    /// </summary>
    /// <returns>A configured <see cref="SpannerInbox"/> instance.</returns>
    /// <exception cref="ConfigurationException">Thrown when configuration is not set.</exception>
    internal SpannerInbox Build()
    {
        if (_configuration == null)
        {
            throw new ConfigurationException("Configuration is null");
        }

        return _connectionProvider == null
            ? new SpannerInbox(_configuration)
            : new SpannerInbox(_configuration, _connectionProvider);
    }
}