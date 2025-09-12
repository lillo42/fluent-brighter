using System;

using Fluent.Brighter.Aws;

using Paramore.Brighter;

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


    public static SnsMessageProducerFactoryBuilder AddPublication<TRequest>(
        this SnsMessageProducerFactoryBuilder builder,
        Action<SnsPublicationBuilder> configure)
        where TRequest : class, IRequest
    {
        var factory = new SnsPublicationBuilder();
        factory.SetRequestType(typeof(TRequest));
        configure(factory);
        return builder.AddPublication(factory.Build());
    }
    
    public static SnsMessageProducerFactoryBuilder AddPublication(this SnsMessageProducerFactoryBuilder builder,
        Action<SnsPublicationBuilder> configure)
    {
        var factory = new SnsPublicationBuilder();
        configure(factory);
        return builder.AddPublication(factory.Build());
    }
}