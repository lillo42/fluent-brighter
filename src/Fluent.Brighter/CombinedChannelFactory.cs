
using System.Collections.Generic;

using Paramore.Brighter;

namespace Fluent.Brighter;

public class CombinedChannelFactory(Dictionary<Subscription, IAmAChannelFactory> channels) : IAmAChannelFactory
{
    public IAmAChannel CreateChannel(Subscription subscription)
    {
        return channels[subscription].CreateChannel(subscription);
    }
}