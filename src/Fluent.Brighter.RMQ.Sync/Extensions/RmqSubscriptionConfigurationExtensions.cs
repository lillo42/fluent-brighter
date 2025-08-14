using System;

using Fluent.Brighter.RMQ.Sync;

using Paramore.Brighter;

namespace Fluent.Brighter;

public static class RmqSubscriptionConfigurationExtensions
{
    public static RmqMessageProducerFactoryBuilder AddPublication(this RmqMessageProducerFactoryBuilder builder,
        Action<RmqPublicationBuilder> configure)
    {
        var publication = new RmqPublicationBuilder();
        configure(publication);
        return builder.AddPublication(publication.Build());
    }
    
    public static RmqMessageProducerFactoryBuilder AddPublication<TRequest>(this RmqMessageProducerFactoryBuilder builder,
        Action<RmqPublicationBuilder> configure)
        where TRequest : class, IRequest
    {
        var publication = new RmqPublicationBuilder();
        publication.SetRequestType(typeof(TRequest));
        configure(publication);
        return builder.AddPublication(publication.Build());
    }
}