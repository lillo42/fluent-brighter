using Fluent.Brighter.Aws;

namespace Fluent.Brighter;

public static class DynamoDbLockingProviderOptionsBuilderExtensions
{
    public static DynamoDbLockingProviderOptionsBuilder EnableManuallyReleaseLock(this DynamoDbLockingProviderOptionsBuilder builder)
    {
        return builder.SetManuallyReleaseLock(true);
    }
    
    public static DynamoDbLockingProviderOptionsBuilder DisableManuallyReleaseLock(this DynamoDbLockingProviderOptionsBuilder builder)
    {
        return builder.SetManuallyReleaseLock(false);
    }
}