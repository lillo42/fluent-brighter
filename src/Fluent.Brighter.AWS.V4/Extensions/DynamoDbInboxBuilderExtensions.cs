using System;

namespace Fluent.Brighter.AWS.V4.Extensions;

public static class DynamoDbInboxBuilderExtensions
{
    public static DynamoDbInboxBuilder SetConnection(this DynamoDbInboxBuilder builder,
        Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new AWSMessagingGatewayConnectionBuilder();
        configure(connection);
        return builder.SetConnection(connection.Build());
    }
}