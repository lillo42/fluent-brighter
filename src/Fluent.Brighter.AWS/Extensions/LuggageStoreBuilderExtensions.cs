using System;

using Fluent.Brighter.AWS;

using Paramore.Brighter.Tranformers.AWS;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for LuggageStoreBuilder to provide fluent configuration
/// for Amazon S3-based luggage storage in Paramore.Brighter.
/// </summary>
public static class LuggageStoreBuilderExtensions
{
    /// <summary>
    /// Configures S3 as the luggage store using a builder pattern for fluent configuration.
    /// </summary>
    /// <param name="builder">The luggage store builder instance</param>
    /// <param name="configure">Action to configure the S3 luggage store builder</param>
    /// <returns>The luggage store builder instance for method chaining</returns>
    public static LuggageStoreBuilder UseS3LuggageStore(this LuggageStoreBuilder builder,
        Action<S3LuggageStoreBuilder> configure)
    {
        var store = new S3LuggageStoreBuilder();
        configure(store);
        return builder.UseS3LuggageStore(store.Build());
    }

    /// <summary>
    /// Configures a pre-built S3 luggage store instance.
    /// </summary>
    /// <param name="builder">The luggage store builder instance</param>
    /// <param name="store">Pre-configured S3 luggage store</param>
    /// <returns>The luggage store builder instance for method chaining</returns>
    public static LuggageStoreBuilder UseS3LuggageStore(this LuggageStoreBuilder builder, S3LuggageStore store)
    {
        builder.UseLuggageStore(store);
        return builder;
    }
}