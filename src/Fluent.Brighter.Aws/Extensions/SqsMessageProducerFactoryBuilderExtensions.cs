using System;

using Amazon.Runtime;

using Fluent.Brighter.Aws;

namespace Fluent.Brighter;

public static class SqsMessageProducerFactoryBuilderExtensions
{
    public static SqsMessageProducerFactoryBuilder SetConfiguration(this SqsMessageProducerFactoryBuilder builder,
        Action<AWSMessagingGatewayConnectionBuidler> configure)
    {
        var factory = new AWSMessagingGatewayConnectionBuidler();
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