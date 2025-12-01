using System;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1.Data;
using Google.Cloud.Storage.V1;

using Paramore.Brighter;
using Paramore.Brighter.Transformers.Gcp;
using Paramore.Brighter.Transforms.Storage;

namespace Fluent.Brighter.GoogleCloud;

/// <summary>
/// Builder class for fluently configuring a Google Cloud Storage (GCS) luggage store for Paramore.Brighter's large message handling.
/// Provides methods to set credentials, bucket configuration, and storage options for storing large messages (luggage) in GCS.
/// </summary>
public sealed class GcsLuggageStoreBuilder
{
    private string _projectId = string.Empty;

    /// <summary>
    /// Sets the Google Cloud project ID where the GCS bucket is located.
    /// </summary>
    /// <param name="projectId">The Google Cloud project ID</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcsLuggageStoreBuilder SetProjectId(string projectId)
    {
        _projectId = projectId;
        return this;
    }

    private Bucket _bucket = new();

    /// <summary>
    /// Sets the GCS bucket configuration object directly, allowing full customization of bucket properties.
    /// </summary>
    /// <param name="bucket">The GCS bucket configuration</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcsLuggageStoreBuilder SetBucket(Bucket bucket)
    {
        _bucket = bucket;
        return this;
    }


    /// <summary>
    /// Sets the name of the GCS bucket where luggage (large messages) will be stored.
    /// </summary>
    /// <param name="bucketName">The name of the GCS bucket</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcsLuggageStoreBuilder SetBucketName(string bucketName)
    {
        _bucket.Name = bucketName;
        return this;
    }

    private ICredential? _credential;

    /// <summary>
    /// Sets the Google Cloud credential to use for authentication when accessing the storage bucket.
    /// </summary>
    /// <param name="credential">The Google Cloud credential</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcsLuggageStoreBuilder SetCredential(ICredential? credential)
    {
        _credential = credential;
        return this;
    }

    private DownloadObjectOptions? _downloadObjectOptions;

    /// <summary>
    /// Sets options for downloading object content when calling <see cref="StorageClient.DownloadObject(string,string,System.IO.Stream,Google.Cloud.Storage.V1.DownloadObjectOptions,System.IProgress{Google.Apis.Download.IDownloadProgress})"/> or <see cref="StorageClient.DownloadObjectAsync(string,string,System.IO.Stream,Google.Cloud.Storage.V1.DownloadObjectOptions,System.Threading.CancellationToken,System.IProgress{Google.Apis.Download.IDownloadProgress})"/>.
    /// Controls download behavior including buffer size and cancellation tokens.
    /// </summary>
    /// <remarks>
    /// Affects stream buffering and progress tracking during payload retrieval operations.
    /// </remarks>
    /// <param name="options">The download object options</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcsLuggageStoreBuilder SetDownloadObjectOptions(DownloadObjectOptions options)
    {
        _downloadObjectOptions = options;
        return this;
    }

    private UploadObjectOptions? _uploadObjectOptions;

    /// <summary>
    /// Sets options for uploading objects when calling <see cref="StorageClient.UploadObject(string,string,string,System.IO.Stream,Google.Cloud.Storage.V1.UploadObjectOptions,System.IProgress{Google.Apis.Upload.IUploadProgress})"/> or <see cref="StorageClient.UploadObjectAsync(string,string,string,System.IO.Stream,Google.Cloud.Storage.V1.UploadObjectOptions,System.Threading.CancellationToken,System.IProgress{Google.Apis.Upload.IUploadProgress})"/>.
    /// Allows configuration of upload behavior including content encoding and predefined ACL settings.
    /// </summary>
    /// <remarks>
    /// Used in all <see cref="GcsLuggageStore"/> operations to control upload resumability and metadata settings.
    /// </remarks>
    /// <param name="options">The upload object options</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcsLuggageStoreBuilder SetUploadObjectOptions(UploadObjectOptions options)
    {
        _uploadObjectOptions = options;
        return this;
    }

    private CreateBucketOptions? _createBucketOptions;

    /// <summary>
    /// Sets options for creating buckets when calling <see cref="StorageClient.CreateBucket(string,Bucket,CreateBucketOptions)"/>.
    /// Controls bucket creation behavior including predefined ACLs and projection settings.
    /// </summary>
    /// <param name="options">The create bucket options</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcsLuggageStoreBuilder SetCreateBucket(CreateBucketOptions options)
    {
        _createBucketOptions = options;
        return this;
    }

    private GetBucketOptions? _getBucketOptions;

    /// <summary>
    /// Sets options for retrieving bucket metadata when calling <see cref="StorageClient.GetBucket(string,GetBucketOptions)"/>.
    /// Controls what bucket information is returned.
    /// </summary>
    /// <param name="options">The get bucket options</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcsLuggageStoreBuilder SetGetBucketOptions(GetBucketOptions options)
    {
        _getBucketOptions = options;
        return this;
    }

    private DeleteObjectOptions? _deleteObjectOptions;

    /// <summary>
    /// Sets options for deleting objects when calling <see cref="StorageClient.DeleteObject(string,string,DeleteObjectOptions)"/>.
    /// Controls deletion behavior including generation matching for safe deletion.
    /// </summary>
    /// <param name="options">The delete object options</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcsLuggageStoreBuilder SetDeleteObjectOptions(DeleteObjectOptions options)
    {
        _deleteObjectOptions = options;
        return this;
    }
    
    private GetObjectOptions? _getObjectOptions;

    /// <summary>
    /// Sets options for retrieving object metadata when calling <see cref="StorageClient.GetObject(string,string,GetObjectOptions)"/>.
    /// Controls what object information is returned including generation and projection settings.
    /// </summary>
    /// <param name="options">The get object options</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcsLuggageStoreBuilder SetGetObjectOptions(GetObjectOptions options)
    {
        _getObjectOptions = options;
        return this;
    }

    private string _prefix = string.Empty;

    /// <summary>
    /// Sets the prefix for luggage objects stored in GCS, which helps organize
    /// and identify Brighter-specific objects in the bucket.
    /// </summary>
    /// <param name="prefix">The prefix for luggage objects</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcsLuggageStoreBuilder SetPrefix(string prefix)
    {
        _prefix = prefix;
        return this;
    }

    private StorageStrategy _strategy = StorageStrategy.Assume;

    /// <summary>
    /// Sets the storage strategy for dealing with missing buckets (Assume, Validate, or Create).
    /// </summary>
    /// <param name="strategy">The storage strategy</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcsLuggageStoreBuilder SetStrategy(StorageStrategy strategy)
    {
        _strategy = strategy;
        return this;
    }

    private Action<StorageClientBuilder>? _clientBuilderConfigurator;

    /// <summary>
    /// Sets a callback to customize the <see cref="StorageClientBuilder"/> before client creation.
    /// Allows advanced configuration of retry policies, scopes, or endpoint settings.
    /// </summary>
    /// <example>
    /// builder.SetClientBuilderConfigurator(clientBuilder => 
    ///     clientBuilder.Scopes = new[] { "https://www.googleapis.com/auth/cloud-platform" });
    /// </example>
    /// <param name="configurator">The configurator action</param>
    /// <returns>The builder instance for method chaining</returns>
    public GcsLuggageStoreBuilder SetClientBuilderConfigurator(Action<StorageClientBuilder> configurator)
    {
        _clientBuilderConfigurator = configurator;
        return this;
    }

    /// <summary>
    /// Builds the GcsLuggageStore instance with the configured options.
    /// </summary>
    /// <returns>A configured <see cref="GcsLuggageStore"/> instance</returns>
    /// <exception cref="ConfigurationException">Thrown if bucket name is not set</exception>
    internal GcsLuggageStore Build()
    {
        return new GcsLuggageStore(
            new GcsLuggageOptions
            {
                Bucket = _bucket,
                ClientBuilderConfigurator = _clientBuilderConfigurator,
                Credential = _credential,
                CreateBucketOptions = _createBucketOptions,
                DeleteObjectOptions = _deleteObjectOptions,
                DownloadObjectOptions = _downloadObjectOptions,
                GetBucketOptions =  _getBucketOptions,
                GetObjectOptions = _getObjectOptions,
                Prefix = _prefix,
                ProjectId = _projectId,
                UploadObjectOptions = _uploadObjectOptions,
                Strategy = _strategy
            });
    }
}