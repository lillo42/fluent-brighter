using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;

namespace Fluent.Brighter.AWS.V4;

/// <summary>
/// Builder class for fluently configuring a redrive policy for Amazon SQS dead-letter queues.
/// Provides methods to set the dead-letter queue name and maximum receive count for messages
/// before they are moved to the dead-letter queue.
/// </summary>
public sealed class RedrivePolicyBuilder
{
    private ChannelName? _deadLetterQueueName;

    /// <summary>
    /// Sets the name of the dead-letter queue where messages will be moved after
    /// exceeding the maximum receive count in the source queue.
    /// </summary>
    /// <param name="deadLetterQueueName">The name of the dead-letter queue</param>
    /// <returns>The builder instance for method chaining</returns>
    public RedrivePolicyBuilder SetDeadLetterQueueName(ChannelName deadLetterQueueName)
    {
        _deadLetterQueueName = deadLetterQueueName;
        return this;
    }


    private int _maxReceiveCount;

    /// <summary>
    /// Sets the maximum number of times a message can be received (attempted to be processed)
    /// before being moved to the dead-letter queue.
    /// </summary>
    /// <param name="maxReceiveCount">The maximum number of receive attempts</param>
    /// <returns>The builder instance for method chaining</returns>
    public RedrivePolicyBuilder SetMaxReceiveCount(int maxReceiveCount)
    {
        _maxReceiveCount = maxReceiveCount;
        return this;
    }

    internal RedrivePolicy Build()
    {
        return new RedrivePolicy(_deadLetterQueueName!, _maxReceiveCount);
    }
}