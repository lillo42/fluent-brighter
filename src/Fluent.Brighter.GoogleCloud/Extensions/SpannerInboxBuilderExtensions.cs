using System;

using Fluent.Brighter.GoogleCloud;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring Spanner inbox functionality.
/// </summary>
public static class SpannerInboxBuilderExtensions
{
    /// <summary>
    /// Configures the Spanner inbox using a fluent configuration builder.
    /// </summary>
    /// <param name="builder">The Spanner inbox builder instance.</param>
    /// <param name="configure">An action to configure the relational database settings.</param>
    /// <returns>The configured <see cref="SpannerInboxBuilder"/> instance for method chaining.</returns>
    public static SpannerInboxBuilder SetConfiguration(this SpannerInboxBuilder builder,
        Action<RelationalDatabaseConfigurationBuilder> configure)
    {
        var configuration = new RelationalDatabaseConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}