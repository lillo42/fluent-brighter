using System;

using MongoDB.Driver.GridFS;

using Paramore.Brighter;
using Paramore.Brighter.MongoDb;
using Paramore.Brighter.Transformers.MongoGridFS;
using Paramore.Brighter.Transforms.Storage;

namespace Fluent.Brighter.MongoDb;

/// <summary>
/// A fluent builder for creating instances of <see cref="MongoDbLuggageStoreOptions"/>.
/// Provides a clean, readable API for configuring MongoDB GridFS storage options.
/// </summary>
public class MongoDbLuggageStoreBuilder
{
    private string? _connectionString;

    /// <summary>
    /// Sets the connection string for the configuration.
    /// </summary>
    /// <param name="connectionString">The MongoDB connection string.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbLuggageStoreBuilder ConnectionString(string connectionString)
    {
        _connectionString = connectionString;
        return this;
    }

    private string? _databaseName;

    /// <summary>
    /// Sets the database name for the configuration.
    /// </summary>
    /// <param name="databaseName">The name of the database.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbLuggageStoreBuilder DatabaseName(string databaseName)
    {
        _databaseName = databaseName;
        return this;
    }

    private string? _bucketName;

    /// <summary>
    /// Sets the bucket name for the configuration.
    /// </summary>
    /// <param name="bucketName">The name of the collection.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbLuggageStoreBuilder BucketName(string bucketName)
    {
        _bucketName = bucketName;
        return this;
    }

    private StorageStrategy _strategy = StorageStrategy.CreateIfMissing;

    /// <summary>
    /// Sets the storage strategy for handling item existence in the store.
    /// Defaults to <see cref="StorageStrategy.CreateIfMissing"/>.
    /// </summary>
    /// <param name="strategy">The strategy to use for storage operations.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbLuggageStoreBuilder Strategy(StorageStrategy strategy)
    {
        _strategy = strategy;
        return this;
    }

    /// <summary>
    /// Sets the storage strategy to <see cref="StorageStrategy.CreateIfMissing"/>.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbLuggageStoreBuilder CreateCollectionIfMissing()
        => Strategy(StorageStrategy.CreateIfMissing);

    /// <summary>
    /// Sets the storage strategy to <see cref="StorageStrategy.Validate"/>.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbLuggageStoreBuilder ValidateIfCollectionExists()
        => Strategy(StorageStrategy.Validate);

    /// <summary>
    /// Sets the storage strategy to <see cref="StorageStrategy.Assume"/>.
    /// </summary>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbLuggageStoreBuilder AssumeCollectionExists()
        => Strategy(StorageStrategy.Assume);

    private GridFSBucketOptions _bucketOptions = new();
    
    /// <summary>
    /// Sets custom GridFS bucket options.
    /// If not explicitly set, defaults to a bucket with the name provided in the constructor.
    /// </summary>
    /// <param name="options">The GridFS bucket options.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbLuggageStoreBuilder BucketOptions(GridFSBucketOptions options)
    {
        _bucketOptions = options ?? throw new ArgumentNullException(nameof(options));
        return this;
    }
    
    private GridFSDownloadByNameOptions? _downloadOptions;

    /// <summary>
    /// Sets custom download options for GridFS operations.
    /// </summary>
    /// <param name="options">The download options.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbLuggageStoreBuilder DownloadOptions(GridFSDownloadByNameOptions? options)
    {
        _downloadOptions = options;
        return this;
    }
    
    
    private GridFSUploadOptions? _uploadOptions;
    
    /// <summary>
    /// Sets custom upload options for GridFS operations.
    /// </summary>
    /// <param name="options">The upload options.</param>
    /// <returns>The current builder instance for fluent chaining.</returns>
    public MongoDbLuggageStoreBuilder UploadOptions(GridFSUploadOptions? options)
    {
        _uploadOptions = options;
        return this;
    }
    
    public MongoDbLuggageStore Build()
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new ConfigurationException("ConnectionString not configured");
        }

        if (string.IsNullOrEmpty(_databaseName))
        {
            throw new ConfigurationException("Database name not configured");
        }

        if (string.IsNullOrEmpty(_bucketName))
        {
            throw new ConfigurationException("Bucket name not configured");
        }

        return new MongoDbLuggageStore(new MongoDbLuggageStoreOptions(_connectionString, _databaseName, _bucketName)
        {
            Strategy = _strategy,
            BucketOptions = _bucketOptions,
            DownloadOptions = _downloadOptions,
            UploadOptions = _uploadOptions
        });
    }
}