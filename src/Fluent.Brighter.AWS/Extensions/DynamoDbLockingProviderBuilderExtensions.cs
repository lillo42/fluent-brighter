using System;

using Fluent.Brighter.AWS;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for DynamoDbLockingProviderBuilder to provide additional configuration options.
/// </summary>
public static class DynamoDbLockingProviderBuilderExtensions
{
    /// <summary>
    /// Sets the AWS connection configuration using a builder pattern for the DynamoDB locking provider.
    /// </summary>
    /// <param name="builder">The DynamoDB locking provider builder instance</param>
    /// <param name="configure">Action to configure the AWS connection builder</param>
    /// <returns>The DynamoDB locking provider builder instance for method chaining</returns>
    public static DynamoDbLockingProviderBuilder SetConnection(this DynamoDbLockingProviderBuilder builder,
        Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new AWSMessagingGatewayConnectionBuilder();
        configure(connection);
        return builder.SetConnection(connection.Build());
    }

    /// <summary>
    /// Sets the locking provider configuration options using a builder pattern.
    /// </summary>
    /// <param name="builder">The DynamoDB locking provider builder instance</param>
    /// <param name="configure">Action to configure the locking provider options builder</param>
    /// <returns>The DynamoDB locking provider builder instance for method chaining</returns>
    public static DynamoDbLockingProviderBuilder SetConfiguration(this DynamoDbLockingProviderBuilder builder,
         Action<DynamoDbLockingProviderOptionsBuilder> configure)
    {
        var connection = new DynamoDbLockingProviderOptionsBuilder();
        configure(connection);
        return builder.SetConfiguration(connection.Build());
    }
}