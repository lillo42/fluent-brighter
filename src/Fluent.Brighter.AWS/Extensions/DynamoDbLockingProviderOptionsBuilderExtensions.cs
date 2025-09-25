using Fluent.Brighter.AWS;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for DynamoDbLockingProviderOptionsBuilder to provide fluent configuration
/// for manual lock release behavior in DynamoDB-based distributed locking.
/// </summary>
public static class DynamoDbLockingProviderOptionsBuilderExtensions
{
    /// <summary>
    /// Enables manual lock release, requiring explicit release of locks rather than
    /// automatic expiration-based release.
    /// </summary>
    /// <param name="builder">The DynamoDB locking provider options builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static DynamoDbLockingProviderOptionsBuilder EnableManuallyReleaseLock(this DynamoDbLockingProviderOptionsBuilder builder)
    {
        return builder.SetManuallyReleaseLock(true);
    }

    /// <summary>
    /// Disables manual lock release, allowing locks to be automatically released
    /// when the lease validity period expires.
    /// </summary>
    /// <param name="builder">The DynamoDB locking provider options builder instance</param>
    /// <returns>The builder instance for method chaining</returns>
    public static DynamoDbLockingProviderOptionsBuilder DisableManuallyReleaseLock(this DynamoDbLockingProviderOptionsBuilder builder)
    {
        return builder.SetManuallyReleaseLock(false);
    }
}