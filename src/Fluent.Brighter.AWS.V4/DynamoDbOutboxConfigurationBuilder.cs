using System;

using Paramore.Brighter.Outbox.DynamoDB;

namespace Fluent.Brighter.AWS.V4;

public sealed class DynamoDbOutboxConfigurationBuilder
{
    private string? _tableName;

    public DynamoDbOutboxConfigurationBuilder SetTableName(string tableName)
    {
        _tableName = tableName;
        return this;
    }
    
    private string? _outstandingIndexName;

    public DynamoDbOutboxConfigurationBuilder SetOutstandingIndexName(string outstandingIndexName)
    {
        _outstandingIndexName = outstandingIndexName;
        return this;
    }
    private TimeSpan? _timeToLive;

    public DynamoDbOutboxConfigurationBuilder SetTimeToLive(TimeSpan? timeToLive)
    {
        _timeToLive = timeToLive;
        return this;
    }
    private int _timeout = 500;
    
    public DynamoDbOutboxConfigurationBuilder SetTimeout(int timeout)
    {
        _timeout = timeout;
        return this;
    }
    private int _numberOfShards = 3;
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