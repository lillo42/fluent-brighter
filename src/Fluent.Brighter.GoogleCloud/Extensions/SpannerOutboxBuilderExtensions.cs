using System;

using Fluent.Brighter.GoogleCloud;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring Spanner outbox functionality.
/// </summary>
public static class SpannerOutboxBuilderExtensions
{
    /// <summary>
    /// Configures the Spanner outbox using a fluent configuration builder.
    /// </summary>
    /// <param name="builder">The Spanner outbox builder instance.</param>
    /// <param name="configure">An action to configure the relational database settings.</param>
    /// <returns>The configured <see cref="SpannerOutboxBuilder"/> instance for method chaining.</returns>
    public static SpannerOutboxBuilder SetConfiguration(this SpannerOutboxBuilder  builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}