using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;
using Paramore.Brighter.Extensions.DependencyInjection;

namespace Fluent.Brighter;

public interface IBrighterConfigurator
{
    IServiceCollection Services { get; }

    BrighterConfiguratorOptions Options { get; }

    AutoFromAssembly FromAssembly { get; set; }

    IBrighterConfigurator AddExternalBus(IAmAMessageProducerFactory messageProducerFactory);

    IBrighterConfigurator AddChannelFactory(IAmAChannelFactory channelFactory, IEnumerable<Subscription> subscriptions);

    Action<IAmAnAsyncSubcriberRegistry> AsyncHandlerRegistry { get; set; }
    Action<IAmASubscriberRegistry> HandlerRegistry { get; set; }

    Action<ServiceCollectionMessageMapperRegistry> MapperRegistry { get; set; }
}