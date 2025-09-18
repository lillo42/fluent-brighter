using System;
using System.Collections.Generic;

using Paramore.Brighter.MessagingGateway.AWSSQS.V4;

namespace Fluent.Brighter.AWS.V4;

public sealed class SqsAttributesBuilder
{
    private TimeSpan? _lockTimeout;
    
    public SqsAttributesBuilder SetLockTimeout(TimeSpan? lockTimeout)
    {
        _lockTimeout = lockTimeout;
        return this;
    }

    private TimeSpan? _timeOut;
    
    public SqsAttributesBuilder SetTimeOut(TimeSpan? timeOut)
    {
        _timeOut = timeOut;
        return this;
    }

    private TimeSpan? _delaySeconds;

    public SqsAttributesBuilder SetDelaySeconds(TimeSpan? delaySeconds)
    {
        _delaySeconds = delaySeconds;
        return this;
    }

    private TimeSpan? _messageRetentionPeriod;
    
    public SqsAttributesBuilder SetMessageRetentionPeriod(TimeSpan? messageRetentionPeriod)
    {
        _messageRetentionPeriod = messageRetentionPeriod;
        return this;
    }

    private string? _iamPolicy;

    public SqsAttributesBuilder SetIamPolicy(string? iamPolicy)
    {
        _iamPolicy = iamPolicy;
        return this;
    }

    private bool _rawMessageDelivery = true;
    
    public SqsAttributesBuilder SetRawMessageDelivery(bool rawMessageDelivery)
    {
        _rawMessageDelivery = rawMessageDelivery;
        return this;
    }

    private RedrivePolicy? _redrivePolicy;

    public SqsAttributesBuilder SetRedrivePolicy(RedrivePolicy? redrivePolicy)
    {
        _redrivePolicy = redrivePolicy;
        return this;
    }

    private Dictionary<string, string>? _tags;
    
    public SqsAttributesBuilder SetTags(Dictionary<string, string>? tags)
    {
        _tags = tags;
        return this;
    }

    public SqsAttributesBuilder AddTag(string key, string value)
    {
        _tags ??= new Dictionary<string, string>();
        _tags.Add(key, value);
        return this;
    }

    private SqsType _type = SqsType.Standard;

    public SqsAttributesBuilder SetType(SqsType type)
    {
        _type = type;
        return this;
    }
    private bool _contentBasedDeduplication = true;

    public SqsAttributesBuilder SetContentBasedDeduplication(bool contentBasedDeduplication)
    {
        _contentBasedDeduplication = contentBasedDeduplication;
        return this;
    }
    private DeduplicationScope? _deduplicationScope;
    public SqsAttributesBuilder SetDeduplicationScope(DeduplicationScope? deduplicationScope)
    {
        _deduplicationScope = deduplicationScope;
        return this;
    }

    private FifoThroughputLimit? _fifoThroughputLimit;
    public SqsAttributesBuilder SetFifoThroughputLimit(FifoThroughputLimit? fifoThroughputLimit)
    {
        _fifoThroughputLimit = fifoThroughputLimit;
        return this;
    }

    internal SqsAttributes Build()
    {
        return new SqsAttributes(
            _lockTimeout,
            _delaySeconds,
            _timeOut,
            _messageRetentionPeriod,
            _iamPolicy,
            _rawMessageDelivery,
            _redrivePolicy,
            _tags,
            _type,
            _contentBasedDeduplication,
            _deduplicationScope,
            _fifoThroughputLimit
        );
    }
}