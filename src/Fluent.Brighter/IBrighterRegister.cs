using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using Paramore.Brighter;

namespace Fluent.Brighter;

public interface IBrighterRegister
{
    IServiceCollection Services { get; }

    BrighterOptions Options { get; }

    IBrighterRegister AddExternalBus(IAmAProducerRegistry producerRegistry);

    IBrighterRegister AddChannelFactory(IAmAChannelFactory channelFactory, IEnumerable<Subscription> subscriptions);
}