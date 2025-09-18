using System;

namespace Fluent.Brighter.AWS.V4.Extensions;

public static class SqsMessageProducerFactoryBuilderExtensions
{
    public static SqsMessageProducerFactoryBuilder SetConfiguration(this SqsMessageProducerFactoryBuilder builder,
        Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var factory = new AWSMessagingGatewayConnectionBuilder();
        configure(factory);
        return builder.SetConfiguration(factory.Build());
    }

    public static SqsMessageProducerFactoryBuilder AddPublication<TRequest>(
        this SqsMessageProducerFactoryBuilder builder,
        Action<SqsPublicationBuilder> configure)
    {
        var factory = new SqsPublicationBuilder();
        factory.SetRequestType(typeof(TRequest));
        configure(factory);
        return builder.AddPublication(factory.Build());
    }

    public static SqsMessageProducerFactoryBuilder AddPublication(this SqsMessageProducerFactoryBuilder builder,
        Action<SqsPublicationBuilder> configure)
    {
        var factory = new SqsPublicationBuilder();
        configure(factory);
        return builder.AddPublication(factory.Build());
    }
}