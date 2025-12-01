using Fluent.Brighter.GoogleCloud;

using Paramore.Brighter.Transforms.Storage;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for <see cref="GcsLuggageStoreBuilder"/> to provide convenient configuration
/// of storage strategies for Google Cloud Storage bucket handling.
/// </summary>
public static class GcsLuggageStoreBuilderExtensions
{
    /// <summary>
    /// Sets the storage strategy to create the GCS bucket if it doesn't exist.
    /// </summary>
    /// <param name="builder">The GCS luggage store builder instance</param>
    /// <returns>The GCS luggage store builder instance for method chaining</returns>
    public static GcsLuggageStoreBuilder CreateIfMissing(this GcsLuggageStoreBuilder builder)
    {
        return builder.SetStrategy(StorageStrategy.CreateIfMissing);
    }

    /// <summary>
    /// Sets the storage strategy to validate that the GCS bucket exists.
    /// </summary>
    /// <param name="builder">The GCS luggage store builder instance</param>
    /// <returns>The GCS luggage store builder instance for method chaining</returns>
    public static GcsLuggageStoreBuilder ValidIfGcsExists(this GcsLuggageStoreBuilder builder)
    {
        return builder.SetStrategy(StorageStrategy.Validate);
    }

    /// <summary>
    /// Sets the storage strategy to assume the GCS bucket exists (no validation).
    /// </summary>
    /// <param name="builder">The GCS luggage store builder instance</param>
    /// <returns>The GCS luggage store builder instance for method chaining</returns>
    public static GcsLuggageStoreBuilder AssumeGcsExists(this GcsLuggageStoreBuilder builder)
    {
        return builder.SetStrategy(StorageStrategy.Assume);
    }
}