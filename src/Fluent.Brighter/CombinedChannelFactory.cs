using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// A composite channel factory that combines multiple channel factories based on subscription types.
/// Delegates channel creation to the appropriate factory registered for each subscription.
/// </summary>
public class CombinedChannelFactory(Dictionary<Subscription, IAmAChannelFactory> channels) : IAmAChannelFactory
{
    /// <inheritdoc />
    public IAmAChannelSync CreateSyncChannel(Subscription subscription)
    {
        return channels[subscription].CreateSyncChannel(subscription);
    }

    /// <inheritdoc />
    public IAmAChannelAsync CreateAsyncChannel(Subscription subscription)
    {
        return channels[subscription].CreateAsyncChannel(subscription);
    }

    /// <inheritdoc />
    public async Task<IAmAChannelAsync> CreateAsyncChannelAsync(Subscription subscription, CancellationToken ct = default)
    {
        return await channels[subscription].CreateAsyncChannelAsync(subscription, ct);
    }
}