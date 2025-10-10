using System;

using MongoDB.Driver.GridFS;

using Paramore.Brighter;
using Paramore.Brighter.Transformers.MongoGridFS;

namespace Fluent.Brighter.MongoDb;

/// <summary>
/// Provides a fluent builder for configuring <see cref="MongoDbLuggageStoreOptions"/>.
/// </summary>
public sealed class MongoDbLuggageStoreBuilder 
{
    private string _connectionString = string.Empty;

    /// <summary>
    /// Sets the MongoDB connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to use.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbLuggageStoreBuilder SetConnectionString(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        return this;
    }
    
    private string _databaseName = string.Empty;

    /// <summary>
    /// Sets the name of the MongoDB database.
    /// </summary>
    /// <param name="databaseName">The database name to use.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbLuggageStoreBuilder SetDatabaseName(string databaseName)
    {
        _databaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
        return this;
    }
    
    private string _bucketName = string.Empty;

    /// <summary>
    /// Sets the name of the GridFS bucket.
    /// </summary>
    /// <param name="bucketName">The bucket name to use.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbLuggageStoreBuilder SetBucketName(string bucketName)
    {
        _bucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
        return this;
    }

    private GridFSDownloadByNameOptions? _downloadOptions;
    
    /// <summary>
    /// Sets the download options for GridFS operations.
    /// </summary>
    /// <param name="options">The download options to use.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbLuggageStoreBuilder SetDownloadOptions(GridFSDownloadByNameOptions? options)
    {
        _downloadOptions = options;
        return this;
    }
    
    private GridFSUploadOptions? _uploadOptions;

    /// <summary>
    /// Sets the upload options for GridFS operations.
    /// </summary>
    /// <param name="options">The upload options to use.</param>
    /// <returns>The current builder instance.</returns>
    public MongoDbLuggageStoreBuilder SetUploadOptions(GridFSUploadOptions? options)
    {
        _uploadOptions = options;
        return this;
    }

    /// <summary>
    /// Builds the <see cref="MongoDbLuggageStoreOptions"/> instance with the configured values.
    /// </summary>
    /// <returns>A new instance of <see cref="MongoDbLuggageStoreOptions"/>.</returns>
    internal MongoDbLuggageStore Build()
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            throw new ConfigurationException("ConnectionString must be provided.");
        }
        
        if (string.IsNullOrEmpty(_databaseName))
        {
            throw new ConfigurationException("DatabaseName must be provided.");
        }
        
        if (string.IsNullOrEmpty(_bucketName))
        {
            throw new ConfigurationException("BucketName must be provided.");
        }

        return new MongoDbLuggageStore(new MongoDbLuggageStoreOptions(_connectionString, _databaseName, _bucketName)
        {
            DownloadOptions = _downloadOptions,
            UploadOptions = _uploadOptions
        });
    }
}