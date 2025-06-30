
using Paramore.Brighter;
using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.Aws;

public class RedrivePolicyBuilder
{
    private int _maxReceiveCount = 3;
    public RedrivePolicyBuilder MaxReceiveCount(int maxReceiveCount)
    {
        _maxReceiveCount = maxReceiveCount;
        return this;
    }


    private string? _deadLetterQueueName;
    public RedrivePolicyBuilder DeadLetterQueueName(string queueName)
    {
        _deadLetterQueueName = queueName;
        return this;
    }

    internal RedrivePolicy Build()
    {
        if (string.IsNullOrEmpty(_deadLetterQueueName))
        {
            throw new ConfigurationException("Missing deadletter");
        }
        
        return new RedrivePolicy(_deadLetterQueueName!, _maxReceiveCount);
    }
}