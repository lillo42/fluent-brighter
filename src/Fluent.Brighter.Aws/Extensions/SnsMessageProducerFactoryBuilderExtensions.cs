using System;

using Fluent.Brighter.Aws;

namespace Fluent.Brighter;

public static class SnsMessageProducerFactoryBuilderExtensions
{
    public static SnsMessageProducerFactoryBuilder SetConfiguration(this SnsMessageProducerFactoryBuilder builder,
        Action<AWSMessagingGatewayConnectionBuilder> configure)
    {
        var factory = new AWSMessagingGatewayConnectionBuilder();
        configure(factory);
        return builder.SetConfiguration(factory.Build());
    }
    
    public static SnsMessageProducerFactoryBuilder AddPublication(this SnsMessageProducerFactoryBuilder builder,
        Action<SnsPublicationBuilder> configure)
    {
        var factory = new SnsPublicationBuilder();
        configure(factory);
        return builder.AddPublication(factory.Build());
    }
}