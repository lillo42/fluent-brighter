using System;
using System.Collections.Generic;

using Paramore.Brighter;

namespace Fluent.Brighter;

/// <summary>
/// The brighter outbox configuration
/// </summary>
public class BrighterOutboxConfiguration
{
    /// <summary>
    /// How many outstanding messages may the outbox have before we terminate the programme with an OutboxLimitReached exception?
    /// -1 => No limit, although the Outbox may discard older entries which is implementation dependent
    /// 0 => No outstanding messages, i.e. throw an error as soon as something goes into the Outbox
    /// 1+ => Allow this number of messages to stack up in an Outbox before throwing an exception (likely to fail fast)
    /// </summary>
    public int? MaxOutStandingMessages { get; set; } = -1;

    /// <summary>
    /// At what interval should we check the number of outstanding messages has not exceeded the limit set in MaxOutStandingMessages
    /// We spin off a thread to check when inserting an item into the outbox, if the interval since the last insertion is greater than this threshold
    /// If you set MaxOutStandingMessages to -1 or 0 this property is effectively ignored
    /// </summary>
    public TimeSpan MaxOutStandingCheckInterval { get; set; } = TimeSpan.Zero;
        
    /// <summary>
    /// The Outbox we wish to use for messaging
    /// </summary>
    public IAmAnOutbox? Outbox { get; set; }
   
    /// <summary>
    /// The maximum amount of messages to deposit into the outbox in one transmissions.
    /// This is to stop insert statements getting too big
    /// </summary>
    public int? OutboxBulkChunkSize { get; set; }
        
    /// <summary>
    /// An outbox may require additional arguments before it can run its checks. The DynamoDb outbox for example expects there to be a Topic in the args
    /// This bag provides the args required
    /// </summary>
    public Dictionary<string, object>? OutBoxBag { get; set; }

    /// <summary>
    /// When do we timeout writing to the outbox
    /// </summary>
    public int? OutboxTimeout { get; set; }
}