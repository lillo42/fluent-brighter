using System;

using Paramore.Brighter;
using Paramore.Brighter.Locking.DynamoDB.V4;

namespace Fluent.Brighter.AWS.V4;

/// <summary>
/// Builder class for fluently configuring options for a DynamoDB-based locking provider.
/// Provides methods to set table name, leaseholder group identification, lease validity duration,
/// and lock release behavior for distributed locking with Amazon DynamoDB.
/// </summary>
public class DynamoDbLockingProviderOptionsBuilder
{
    private string? _tableName;

    /// <summary>
    /// Sets the name of the DynamoDB table used for storing lock information.
    /// </summary>
    /// <param name="tableName">The name of the DynamoDB table</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbLockingProviderOptionsBuilder SetTableName(string tableName)
    {
        _tableName = tableName;
        return this;
    }

    private string? _leaseholderGroupId;

    ///  <summary>
    /// Sets the leaseholder group identifier which distinguishes different groups
    /// of consumers that may be competing for the same locks.
    /// </summary>
    /// <param name="leaseholderGroupId">The leaseholder group identifier</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbLockingProviderOptionsBuilder SetLeaseholderGroupId(string leaseholderGroupId)
    {
        _leaseholderGroupId = leaseholderGroupId;
        return this;
    }

    private TimeSpan _leaseValidity = TimeSpan.FromHours(1);

    /// <summary>
    /// Sets the validity duration for acquired locks before they automatically expire.
    /// </summary>
    /// <param name="leaseValidity">The duration before locks expire</param>
    /// <returns>The builder instance for method chaining</returns>
    public DynamoDbLockingProviderOptionsBuilder SetLeaseValidity(TimeSpan leaseValidity)
    {
        _leaseValidity = leaseValidity;
        return this;
    }

    private bool _manuallyReleaseLock = false;

    /// <summary>
    /// Sets whether locks must be manually released or will be automatically released
    /// when the lease validity period expires.
    /// </summary>
    /// <param name="manuallyReleaseLock">True to require manual lock release</param>
    /// <returns>The builder instance for method chaining</returns>
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