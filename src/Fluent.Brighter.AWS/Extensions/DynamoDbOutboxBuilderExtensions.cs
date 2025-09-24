using System;

using Fluent.Brighter.AWS;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for DynamoDbOutboxBuilder to provide additional configuration options.
/// </summary>
public static class DynamoDbOutboxBuilderExtensions
{
    /// <summary>
    /// Sets the AWS connection configuration using a builder pattern for the DynamoDB outbox.
    /// </summary>
    /// <param name="builder">The DynamoDB outbox builder instance</param>
    /// <param name="configure">Action to configure the AWS connection builder</param>
    /// <returns>The DynamoDB outbox builder instance for method chaining</returns>
    public static DynamoDbOutboxBuilder SetConnection(this DynamoDbOutboxBuilder builder,
        Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new AWSMessagingGatewayConnectionBuilder();
        configure(connection);
        return builder.SetConnection(connection.Build());
    }

    /// <summary>
    /// Sets the outbox configuration using a builder pattern for the DynamoDB outbox.
    /// </summary>
    /// <param name="builder">The DynamoDB outbox builder instance</param>
    /// <param name="configure">Action to configure the outbox configuration builder</param>
    /// <returns>The DynamoDB outbox builder instance for method chaining</returns>
    public static DynamoDbOutboxBuilder SetConfiguration(this DynamoDbOutboxBuilder builder,
        Action<DynamoDbOutboxConfigurationBuilder> configure)
    {
        var configuration = new DynamoDbOutboxConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}