using System;

using Fluent.Brighter.Aws;

namespace Fluent.Brighter;

public static class DynamoDbOutboxBuilderExtensions
{
    public static DynamoDbOutboxBuilder SetConnection(this DynamoDbOutboxBuilder builder,
        Action<AWSMessagingGatewayConnectionBuidler> configure)
    {
        var connection = new  AWSMessagingGatewayConnectionBuidler();
        configure(connection);
        return builder.SetConnection(connection.Build());
    }
    
    public static DynamoDbOutboxBuilder SetConfiguration(this DynamoDbOutboxBuilder builder,
        Action<DynamoDbOutboxConfigurationBuilder> configure)
    {
        var configuration = new  DynamoDbOutboxConfigurationBuilder();
        configure(configuration);
        return builder.SetConfiguration(configuration.Build());
    }
}