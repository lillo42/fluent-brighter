
using System.Collections.Generic;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// A composite channel factory that combines multiple channel factories based on subscription types.
/// Delegates channel creation to the appropriate factory registered for each subscription.
/// </summary>
public class CombinedChannelFactory(Dictionary<Subscription, IAmAChannelFactory> channels) : IAmAChannelFactory
{
    /// <inheritdoc cref="IAmAChannelFactory.CreateChannel(Subscription)"/>,
    public IAmAChannel CreateChannel(Subscription subscription)
    {
        return channels[subscription].CreateChannel(subscription);
    }
}