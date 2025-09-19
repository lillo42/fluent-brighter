using System;
using Paramore.Brighter.Outbox.DynamoDB;

namespace Fluent.Brighter.Aws;

/// <summary>
/// Builder class for fluently configuring DynamoDB outbox settings.
/// Provides methods to set table name, index names, timeout values, TTL settings,
/// and shard configuration for optimizing DynamoDB outbox performance and functionality.
/// </summary>
public sealed class DynamoDbOutboxConfigurationBuilder
{
    private string? _tableName;

    /// <summary>
    /// Sets the name of the DynamoDB table used for the outbox.
    /// </summary>
    /// <param name="tableName">The name of the DynamoDB table</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbOutboxConfigurationBuilder SetTableName(string tableName)
    {
        _tableName = tableName;
        return this;
    }
    
    private string? _outstandingIndexName;

    /// <summary>
    /// Sets the name of the Global Secondary Index (GSI) used for querying outstanding messages.
    /// </summary>
    /// <param name="outstandingIndexName">The name of the outstanding messages index</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbOutboxConfigurationBuilder SetOutstandingIndexName(string outstandingIndexName)
    {
        _outstandingIndexName = outstandingIndexName;
        return this;
    }
    
    private TimeSpan? _timeToLive;

    /// <summary>
    /// Sets the Time to Live (TTL) for messages in the outbox, after which they will be
    /// automatically deleted by DynamoDB.
    /// </summary>
    /// <param name="timeToLive">The time before messages are automatically expired</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbOutboxConfigurationBuilder SetTimeToLive(TimeSpan? timeToLive)
    {
        _timeToLive = timeToLive;
        return this;
    }
    
    private int _timeout = 500;
    
    /// <summary>
    /// Sets the timeout for DynamoDB operations in milliseconds.
    /// </summary>
    /// <param name="timeout">The operation timeout in milliseconds</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbOutboxConfigurationBuilder SetTimeout(int timeout)
    {
        _timeout = timeout;
        return this;
    }
    
    private int _numberOfShards = 3;
    
    /// <summary>
    /// Sets the number of shards to use for the outbox, which helps distribute
    /// write load and improve throughput in high-volume scenarios.
    /// </summary>
    /// <param name="numberOfShards">The number of shards to create</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbOutboxConfigurationBuilder SetNumberOfShards(int numberOfShards)
    {
        _numberOfShards = numberOfShards;
        return this;
    }

    internal DynamoDbConfiguration Build()
    {
        var config = new DynamoDbConfiguration(_tableName, _timeout, _numberOfShards)
        {
            TimeToLive = _timeToLive
        };
        
        if (!string.IsNullOrEmpty(_outstandingIndexName))
        {
            config.OutstandingIndexName = _outstandingIndexName;
        }
        
        return config;
    }
}