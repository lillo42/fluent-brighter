using System;

using Amazon.Runtime;

using Fluent.Brighter.Aws;

namespace Fluent.Brighter;

public static class SqsMessageProducerFactoryBuilderExtensions
{
    public static SqsMessageProducerFactoryBuilder SetConfiguration(this SqsMessageProducerFactoryBuilder builder,
        Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var factory = new AWSMessagingGatewayConnectionBuilder();
        configure(factory);
        return builder.SetConfiguration(factory.Build());
    }

    public static SqsMessageProducerFactoryBuilder AddPublication(this SqsMessageProducerFactoryBuilder builder,
        Action<SqsPublicationBuilder> configure)
    {
        var factory = new SqsPublicationBuilder();
        configure(factory);
        return builder.AddPublication(factory.Build());
    }
}