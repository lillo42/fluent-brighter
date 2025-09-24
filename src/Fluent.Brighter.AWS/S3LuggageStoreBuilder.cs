using System.Collections.Generic;
using System.Net.Http;

using Amazon.S3;
using Amazon.S3.Model;

using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;
using Paramore.Brighter.Tranformers.AWS;
using Paramore.Brighter.Transforms.Storage;

using Polly.Retry;

namespace Fluent.Brighter.AWS;

/// <summary>
/// Builder class for fluently configuring an S3 luggage store for Paramore.Brighter's large message handling.
/// Provides methods to set AWS connection, bucket configuration, storage policies, and lifecycle management
/// for storing large messages (luggage) in Amazon S3.
/// </summary>
public sealed class S3LuggageStoreBuilder
{
    private AWSS3Connection? _connection;

    /// <summary>
    /// Sets the AWS S3 connection configuration using an existing messaging gateway connection.
    /// Automatically derives the S3 region from the provided connection if not explicitly set.
    /// </summary>
    /// <param name="connection">The AWS messaging gateway connection</param>
    /// <returns>The builder instance for method chaining</returns>
    public S3LuggageStoreBuilder SetConnection(AWSMessagingGatewayConnection connection)
    {
        _connection = new AWSS3Connection(connection.Credentials, connection.Region, connection.ClientConfigAction);
        if (_s3Region == null)
        {
            _s3Region = new S3Region(connection.Region.SystemName);
        }

        return this;
    }

    private string? _bucketName;

    /// <summary>
    /// Sets the name of the S3 bucket where luggage (large messages) will be stored.
    /// </summary>
    /// <param name="bucketName">The name of the S3 bucket</param>
    /// <returns>The builder instance for method chaining</returns>
    public S3LuggageStoreBuilder SetBucketName(string bucketName)
    {
        _bucketName = bucketName;
        return this;
    }

    private S3CannedACL? _acls;

    /// <summary>
    /// Sets the canned ACL (Access Control List) for objects stored in S3.
    /// </summary>
    /// <param name="acls">The S3 canned ACL setting</param>
    /// <returns>The builder instance for method chaining</returns>
    public S3LuggageStoreBuilder SetACLs(S3CannedACL acls)
    {
        _acls = acls;
        return this;
    }

    private string _luggagePrefix = "BRIGHTER_CHECKED_LUGGAGE";

    /// <summary>
    /// Sets the prefix for luggage objects stored in S3, which helps organize
    /// and identify Brighter-specific objects in the bucket.
    /// </summary>
    /// <param name="luggagePrefix">The prefix for luggage objects</param>
    /// <returns>The builder instance for method chaining</returns>
    public S3LuggageStoreBuilder SetLuggagePrefix(string luggagePrefix)
    {
        _luggagePrefix = luggagePrefix;
        return this;
    }

    private IHttpClientFactory? _httpClientFactory;

    /// <summary>
    /// Sets the HTTP client factory for creating HTTP clients used in S3 operations.
    /// </summary>
    /// <param name="httpClientFactory">The HTTP client factory</param>
    /// <returns>The builder instance for method chaining</returns>
    public S3LuggageStoreBuilder SetHttpClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        return this;
    }

    private List<Tag> _tags = [new() { Key = "Creator", Value = " Brighter Luggage Store" }];

    /// <summary>
    /// Sets the tags to be applied to S3 objects, which help with organization,
    /// cost allocation, and access control.
    /// </summary>
    /// <param name="tags">List of tags to apply</param>
    /// <returns>The builder instance for method chaining</returns>
    public S3LuggageStoreBuilder SetTags(List<Tag> tags)
    {
        _tags = tags;
        return this;
    }

    /// <summary>
    /// Adds a single tag to be applied to S3 objects.
    /// </summary>
    /// <param name="tag">The tag to add</param>
    /// <returns>The builder instance for method chaining</returns>
    public S3LuggageStoreBuilder AddTag(Tag tag)
    {
        _tags.Add(tag);
        return this;
    }

    private int _timeToAbortFailedUploads = 1;

    /// <summary>
    /// Sets the time (in days) after which failed uploads should be aborted and cleaned up.
    /// </summary>
    /// <param name="days">Number of days before aborting failed uploads</param>
    /// <returns>The builder instance for method chaining</returns>
    public S3LuggageStoreBuilder SetTimeToAbortFailedUploads(int days)
    {
        _timeToAbortFailedUploads = days;
        return this;
    }

    private int _timeToDeleteGoodUploads = 7;

    /// <summary>
    /// Sets the time (in days) after which successfully uploaded luggage should be deleted from S3.
    /// </summary>
    /// <param name="days">Number of days before deleting successful uploads</param>
    /// <returns>The builder instance for method chaining</returns>
    public S3LuggageStoreBuilder SetTimeToDeleteGoodUploads(int days)
    {
        _timeToDeleteGoodUploads = days;
        return this;
    }

    private string _bucketAddressTemplate = S3LuggageOptions.DefaultBucketAddressTemplate;

    /// <summary>
    /// Sets the template for generating S3 bucket addresses, which can be customized
    /// for different S3-compatible storage providers or specific addressing schemes.
    /// </summary>
    /// <param name="bucketAddressTemplate">The bucket address template</param>
    /// <returns>The builder instance for method chaining</returns>
    public S3LuggageStoreBuilder SetBucketAddressTemplate(string bucketAddressTemplate)
    {
        _bucketAddressTemplate = bucketAddressTemplate;
        return this;
    }

    private AsyncRetryPolicy? _retryPolicy;

    /// <summary>
    /// Sets the retry policy for S3 operations, allowing customization of retry behavior
    /// for transient failures.
    /// </summary>
    /// <param name="retryPolicy">The async retry policy</param>
    /// <returns>The builder instance for method chaining</returns>
    public S3LuggageStoreBuilder SetRetryPolicy(AsyncRetryPolicy? retryPolicy)
    {
        _retryPolicy = retryPolicy;
        return this;
    }

    private StorageStrategy _strategy = StorageStrategy.Assume;

    /// <summary>
    /// Sets the storage strategy for dealing with missing buckets (Assume, Validate, or Create).
    /// </summary>
    /// <param name="strategy">The storage strategy</param>
    /// <returns>The builder instance for method chaining</returns>
    public S3LuggageStoreBuilder SetStrategy(StorageStrategy strategy)
    {
        _strategy = strategy;
        return this;
    }

    private S3Region? _s3Region;

    /// <summary>
    /// Sets the AWS region for the S3 bucket, overriding any region derived from the connection.
    /// </summary>
    /// <param name="s3Region">The S3 region</param>
    /// <returns>The builder instance for method chaining</returns>
    public S3LuggageStoreBuilder SetRegion(S3Region s3Region)
    {
        _s3Region = s3Region;
        return this;
    }

    internal S3LuggageStore Build()
    {
        if (_connection == null)
        {
            throw new ConfigurationException("Connection is null");
        }

        if (string.IsNullOrEmpty(_bucketName))
        {
            throw new ConfigurationException("Bucket name is null or empty");
        }

        return new S3LuggageStore(new S3LuggageOptions(_connection, _bucketName!)
        {
            ACLs = _acls,
            LuggagePrefix = _luggagePrefix,
            HttpClientFactory = _httpClientFactory,
            Tags = _tags,
            TimeToAbortFailedUploads = _timeToAbortFailedUploads,
            TimeToDeleteGoodUploads = _timeToDeleteGoodUploads,
            BucketAddressTemplate = _bucketAddressTemplate,
            RetryPolicy = _retryPolicy,
            Strategy = _strategy,
            BucketRegion = _s3Region!,
        });
    }
}