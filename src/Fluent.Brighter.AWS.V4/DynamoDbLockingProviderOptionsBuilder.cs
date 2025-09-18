using System;

using Paramore.Brighter;
using Paramore.Brighter.Locking.DynamoDB.V4;

namespace Fluent.Brighter.AWS.V4;

public class DynamoDbLockingProviderOptionsBuilder
{
    private string? _tableName;

    public DynamoDbLockingProviderOptionsBuilder SetTableName(string tableName)
    {
        _tableName = tableName;
        return this;
    }

    private string? _leaseholderGroupId;

    public DynamoDbLockingProviderOptionsBuilder SetLeaseholderGroupId(string leaseholderGroupId)
    {
        _leaseholderGroupId = leaseholderGroupId;
        return this;
    }
    
    private TimeSpan _leaseValidity = TimeSpan.FromHours(1);

    public DynamoDbLockingProviderOptionsBuilder SetLeaseValidity(TimeSpan leaseValidity)
    {
        _leaseValidity = leaseValidity;
        return this;
    }
    
    private bool _manuallyReleaseLock = false;
    
    public DynamoDbLockingProviderOptionsBuilder SetManuallyReleaseLock(bool manuallyReleaseLock)
    {
        _manuallyReleaseLock = manuallyReleaseLock;
        return this;
    }
    
    internal DynamoDbLockingProviderOptions Build()
    {
        if (!string.IsNullOrEmpty(_tableName))
        {
            throw new ConfigurationException("TableName was not set");
        }

        if (!string.IsNullOrEmpty(_leaseholderGroupId))
        {
            throw new ConfigurationException("LeaseholderGroupId was not set");
        }

        return new DynamoDbLockingProviderOptions(_tableName!, _leaseholderGroupId!)
        {
            LeaseValidity = _leaseValidity,
            ManuallyReleaseLock = _manuallyReleaseLock
        };
    }
}