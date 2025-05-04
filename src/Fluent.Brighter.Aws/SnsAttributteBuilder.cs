
using System.Collections.Generic;

using Amazon.SimpleNotificationService.Model;

using Paramore.Brighter.MessagingGateway.AWSSQS;

namespace Fluent.Brighter.Aws;

public class SnsAttributesBuilder
{
    private string? _deliveryPolicy;

    public SnsAttributesBuilder DeliveryPolicy(string deliveryPolicy)
    {
        _deliveryPolicy = deliveryPolicy;
        return this;
    }

    private string? _policy;
    public SnsAttributesBuilder Policy(string policy)
    {
        _policy = policy;
        return this;
    }

    private readonly List<Tag> _tags = [];
    public SnsAttributesBuilder Tags(params Tag[] tags)
    {
        _tags.AddRange(tags);
        return this;
    }

    public SnsAttributesBuilder Tags(IEnumerable<Tag> tags)
    {
        _tags.AddRange(tags);
        return this;
    }

    internal SnsAttributes Build()
    {
        var attributes = new SnsAttributes
        {
            DeliveryPolicy = _deliveryPolicy,
            Policy = _policy
        };

        attributes.Tags.AddRange(_tags);

        return attributes;
    }
}