using System;
using System.Collections.Generic;
using System.Linq;

using Amazon.S3.Model;

using Fluent.Brighter.AWS;

using Paramore.Brighter.Transforms.Storage;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for S3LuggageStoreBuilder to provide additional configuration options
/// and convenience methods for Amazon S3-based luggage storage.
/// </summary>
public static class S3LuggageStoreBuilderExtensions
{
    /// <summary>
    /// Sets the AWS connection configuration using a builder pattern for the S3 luggage store.
    /// </summary>
    /// <param name="builder">The S3 luggage store builder instance</param>
    /// <param name="configure">Action to configure the AWS connection builder</param>
    /// <returns>The S3 luggage store builder instance for method chaining</returns>
    public static S3LuggageStoreBuilder SetConnection(this S3LuggageStoreBuilder builder,
        Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new AWSMessagingGatewayConnectionBuilder();
        configure(connection);
        return builder.SetConnection(connection.Build());
    }

    /// <summary>
    /// Sets tags for S3 objects from a collection of tags.
    /// </summary>
    /// <param name="builder">The S3 luggage store builder instance</param>
    /// <param name="tags">Collection of tags to apply to S3 objects</param>
    /// <returns>The S3 luggage store builder instance for method chaining</returns>
    public static S3LuggageStoreBuilder SetTags(this S3LuggageStoreBuilder builder, IEnumerable<Tag> tags)
    {
        return builder.SetTags(tags.ToList());
    }

    /// <summary>
    /// Sets tags for S3 objects from a parameter array of tags.
    /// </summary>
    /// <param name="builder">The S3 luggage store builder instance</param>
    /// <param name="tags">Array of tags to apply to S3 objects</param>
    /// <returns>The S3 luggage store builder instance for method chaining</returns>
    public static S3LuggageStoreBuilder SetTags(this S3LuggageStoreBuilder builder, params Tag[] tags)
    {
        return builder.SetTags(tags.ToList());
    }

    /// <summary>
    /// Adds a single tag to S3 objects using a key-value pair.
    /// </summary>
    /// <param name="builder">The S3 luggage store builder instance</param>
    /// <param name="key">The tag key</param>
    /// <param name="value">The tag value</param>
    /// <returns>The S3 luggage store builder instance for method chaining</returns>
    public static S3LuggageStoreBuilder AddTag(this S3LuggageStoreBuilder builder, string key, string value)
    {
        return builder.AddTag(new Tag { Key = key, Value = value });
    }

    /// <summary>
    /// Sets the storage strategy to create the S3 bucket if it doesn't exist.
    /// </summary>
    /// <param name="builder">The S3 luggage store builder instance</param>
    /// <returns>The S3 luggage store builder instance for method chaining</returns>
    public static S3LuggageStoreBuilder CreateIfMissing(this S3LuggageStoreBuilder builder)
    {
        return builder.SetStrategy(StorageStrategy.CreateIfMissing);
    }

    /// <summary>
    /// Sets the storage strategy to validate that the S3 bucket exists.
    /// </summary>
    /// <param name="builder">The S3 luggage store builder instance</param>
    /// <returns>The S3 luggage store builder instance for method chaining</returns>
    public static S3LuggageStoreBuilder ValidIfS3Exists(this S3LuggageStoreBuilder builder)
    {
        return builder.SetStrategy(StorageStrategy.Validate);
    }

    /// <summary>
    /// Sets the storage strategy to assume the S3 bucket exists (no validation).
    /// </summary>
    /// <param name="builder">The S3 luggage store builder instance</param>
    /// <returns>The S3 luggage store builder instance for method chaining</returns>
    public static S3LuggageStoreBuilder AssumeS3Exists(this S3LuggageStoreBuilder builder)
    {
        return builder.SetStrategy(StorageStrategy.Assume);
    }
}