using Paramore.Brighter.MessagingGateway.AWSSQS.V4;

namespace Fluent.Brighter.AWS.V4;

public sealed class SnsAttributesBuilder
{
    private string? _deliveryPolicy;

    public SnsAttributesBuilder SetDeliveryPolicy(string? deliveryPolicy)
    {
        _deliveryPolicy = deliveryPolicy;
        return this;
    }

    private string? _policy;
    public SnsAttributesBuilder SetPolicy(string? policy)
    {
        _policy = policy;
        return this;
    }

    private SqsType _type = SqsType.Standard;
    public SnsAttributesBuilder SetType(SqsType type)
    {
        _type = type;
        return this;
    }

    private bool _contentBasedDeduplication = true;
    public SnsAttributesBuilder SetContentBasedDeduplication(bool contentBasedDeduplication)
    {
        _contentBasedDeduplication = contentBasedDeduplication;
        return this;
    }

    internal SnsAttributes Build()
    {
        return new SnsAttributes(_deliveryPolicy, _policy, _type, _contentBasedDeduplication);
    }
}