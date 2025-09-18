using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS.V4;

namespace Fluent.Brighter.AWS.V4;

public sealed class RedrivePolicyBuilder
{
    private ChannelName? _deadLetterQueueName;

    public RedrivePolicyBuilder SetDeadLetterQueueName(ChannelName deadLetterQueueName)
    {
        _deadLetterQueueName = deadLetterQueueName;
        return this;
    }


    private int _maxReceiveCount;
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