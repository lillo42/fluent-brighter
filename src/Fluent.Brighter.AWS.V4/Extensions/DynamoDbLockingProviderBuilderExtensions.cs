using System;

namespace Fluent.Brighter.AWS.V4.Extensions;

public static class DynamoDbLockingProviderBuilderExtensions
{
    public static DynamoDbLockingProviderBuilder SetConnection(this DynamoDbLockingProviderBuilder builder,
        Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var connection = new AWSMessagingGatewayConnectionBuilder();
        configure(connection);
        return builder.SetConnection(connection.Build());
    }
    
   public static DynamoDbLockingProviderBuilder SetConfiguration(this DynamoDbLockingProviderBuilder builder,
        Action<DynamoDbLockingProviderOptionsBuilder> configure)
    {
        var connection = new DynamoDbLockingProviderOptionsBuilder();
        configure(connection);
        return builder.SetConfiguration(connection.Build());
    } 
}