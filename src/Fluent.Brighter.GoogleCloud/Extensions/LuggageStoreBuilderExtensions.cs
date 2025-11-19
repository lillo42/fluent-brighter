using System;

using Fluent.Brighter.GoogleCloud;

using Paramore.Brighter.Transformers.Gcp;

namespace Fluent.Brighter;

/// <summary>
/// Provides extension methods for configuring Google Cloud Storage as a luggage store with the <see cref="LuggageStoreBuilder"/>.
/// </summary>
public static class LuggageStoreBuilderExtensions
{
    /// <summary>
    /// Configures the luggage store to use Google Cloud Storage (GCS) with a builder pattern.
    /// </summary>
    /// <param name="builder">The luggage store builder instance.</param>
    /// <param name="configure">An action to configure the <see cref="GcsLuggageStoreBuilder"/>.</param>
    /// <returns>The luggage store builder for method chaining.</returns>
    public static LuggageStoreBuilder UseGcsLuggageStore(this LuggageStoreBuilder builder,
        Action<GcsLuggageStoreBuilder> configure)
    {
        var store = new GcsLuggageStoreBuilder();
        configure(store);
        return builder;
    }
    
    /// <summary>
    /// Configures the luggage store to use a pre-configured Google Cloud Storage (GCS) luggage store instance.
    /// </summary>
    /// <param name="builder">The luggage store builder instance.</param>
    /// <param name="store">The configured <see cref="GcsLuggageStore"/> instance.</param>
    /// <returns>The luggage store builder for method chaining.</returns>
    public static LuggageStoreBuilder UseGcsLuggageStore(this LuggageStoreBuilder builder, GcsLuggageStore store)
    {
        builder.UseLuggageStore(store);
        return builder;
    }
}