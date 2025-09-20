using System;

using Fluent.Brighter.Aws;

namespace Fluent.Brighter;

/// <summary>
/// Extension methods for DynamoDbInboxBuilder to provide additional configuration options.
/// </summary>
public static class DynamoDbInboxBuilderExtensions
{
    /// <summary>
    /// Sets the AWS connection configuration using a builder pattern for the DynamoDB inbox.
    /// </summary>
    /// <param name="builder">The DynamoDB inbox builder instance</param>
    /// <param name="configure">Action to configure the AWS connection builder</param>
    /// <returns>The DynamoDB inbox builder instance for method chaining</returns>
    public static DynamoDbInboxBuilder SetConnection(this DynamoDbInboxBuilder builder,
        Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new AWSMessagingGatewayConnectionBuilder();
        configure(connection);
        return builder.SetConnection(connection.Build());
    }
}